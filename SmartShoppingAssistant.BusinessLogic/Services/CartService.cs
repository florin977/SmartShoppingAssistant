using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartService(ICartRepository cartRepository, IMapper mapper) : ICartService
    {
        // Only active promotions return from the repository, so no need to check for IsActive here.
        private static (Dictionary<int, (int Quantity, decimal Total)> byProduct,
                Dictionary<int, (int Quantity, decimal Total)> byCategory)
    ComputeCartAggregates(IEnumerable<CartItemGetDTO> items)
        {
            var byProduct = items
                .GroupBy(i => i.Product.Id)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        Quantity: g.Sum(i => i.Quantity),
                        Total: g.Sum(i => i.Quantity * i.Product.Price)
                    )
                );

            var byCategory = items
                .SelectMany(i => i.Product.Categories, (i, c) => (Category: c, Item: i))
                .GroupBy(x => x.Category.Id)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        Quantity: g.Sum(x => x.Item.Quantity),
                        Total: g.Sum(x => x.Item.Quantity * x.Item.Product.Price)
                    )
                );

            return (byProduct, byCategory);
        }

        private static IEnumerable<PromotionGetDTO> GetAllPromotions(CartItemGetDTO item)
        {
            var productPromotions = item.Product.Promotions ?? Enumerable.Empty<PromotionGetDTO>();

            var categoryPromotions = item.Product.Categories
                .SelectMany(c => c.Promotions ?? Enumerable.Empty<PromotionGetDTO>());

            return productPromotions
                .Concat(categoryPromotions)
                .DistinctBy(p => p.Id);
        }

        private static bool IsEligible(
            PromotionGetDTO p,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            return p.Type switch
            {
                PromotionType.Quantity =>
                    p.ProductId.HasValue
                        ? byProduct.TryGetValue(p.ProductId.Value, out var prod) && prod.Quantity >= p.Threshold
                        : p.CategoryId.HasValue
                            ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) && cat.Quantity >= p.Threshold
                            : false,

                PromotionType.CartTotal =>
                    p.ProductId.HasValue
                        ? byProduct.TryGetValue(p.ProductId.Value, out var prod) && prod.Total >= p.Threshold
                        : p.CategoryId.HasValue
                            ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) && cat.Total >= p.Threshold
                            : cartTotal >= p.Threshold,

                _ => false
            };
        }

        private static IEnumerable<PromotionGetDTO> GetEligiblePromotions(
            IEnumerable<CartItemGetDTO> items,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            return items
                .SelectMany(GetAllPromotions)
                .DistinctBy(p => p.Id)
                .Where(p => IsEligible(p, byProduct, byCategory, cartTotal));
        }

        private static (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) ComputePromoResult(
            PromotionGetDTO p,
            IEnumerable<CartItemGetDTO> items,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            switch (p.Reward)
            {
                case PromotionReward.PercentDiscount:
                    {
                        decimal applicableTotal = p.ProductId.HasValue
                            ? byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Total : 0
                            : p.CategoryId.HasValue
                                ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Total : 0
                                : cartTotal;

                        return (applicableTotal * (p.RewardValue / 100m), 0, null);
                    }

                case PromotionReward.FreeItems:
                    {
                        if (p.ProductId.HasValue)
                        {
                            byProduct.TryGetValue(p.ProductId.Value, out var prod);
                            decimal unitPrice = prod.Total / prod.Quantity;
                            return (p.RewardValue * unitPrice, p.RewardValue, p.ProductId);
                        }

                        if (p.CategoryId.HasValue)
                        {
                            // Placeholder: cheapest product in the category gets the free items
                            var cheapest = items
                                .Where(i => i.Product.Categories.Any(c => c.Id == p.CategoryId.Value))
                                .OrderBy(i => i.Product.Price)
                                .FirstOrDefault();

                            if (cheapest is null)
                                return (0, 0, null);

                            return (p.RewardValue * cheapest.Product.Price, p.RewardValue, cheapest.Product.Id);
                        }

                        return (0, 0, null);
                    }

                default:
                    return (0, 0, null);
            }
        }

        private static (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) GetBestPromotion(
            IEnumerable<CartItemGetDTO> items,
            decimal cartTotal)
        {
            var itemList = items.ToList();
            var (byProduct, byCategory) = ComputeCartAggregates(itemList);

            return GetEligiblePromotions(itemList, byProduct, byCategory, cartTotal)
                .Select(p => ComputePromoResult(p, itemList, byProduct, byCategory, cartTotal))
                .OrderByDescending(r => r.TotalSaved)
                .FirstOrDefault();
        }

        public async Task<CartGetDTO> GetCartAsync(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            var cartDTO = mapper.Map<CartGetDTO>(cart);
            cartDTO.TotalBeforeDiscount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) = GetBestPromotion(cartDTO.CartItems, cartDTO.TotalBeforeDiscount);
            if (FreeItemProductId.HasValue)
            {
                var freeItem = cartDTO.CartItems.FirstOrDefault(ci => ci.Product.Id == FreeItemProductId.Value);
                if (freeItem != null)
                {
                    cartDTO.TotalBeforeDiscount += FreeItemQuantity * freeItem.Product.Price; // Add free item value to total before discount
                    freeItem.Quantity += FreeItemQuantity;
                }
                else
                {
                    throw new Exception("Promotion logic error: Free item product not found in cart");
                }
            }

            cartDTO.TotalAfterDiscount = cartDTO.TotalBeforeDiscount - TotalSaved;

            return cartDTO;
        }

        public async Task<CartGetDTO> AddItemToCartAsync(int userId, CartItemPostDTO cartItemPostDTO)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);

            // TODO: Remove after implementing User authentication and authorization, or not.
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await cartRepository.AddAsync(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemPostDTO.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += cartItemPostDTO.Quantity;
            }
            else
            {
                var newCartItem = mapper.Map<CartItem>(cartItemPostDTO);
                cart.CartItems.Add(newCartItem);
            }

            await cartRepository.UpdateAsync(cart);
            return mapper.Map<CartGetDTO>(cart);
        }

        public async Task<CartGetDTO> UpdateItemFromCartAsync(int userId, int itemId, CartItemPutDTO cartItemPutDTO)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);

            if (cartItem == null)
            {
                throw new Exception("Cart item not found");
            }

            cartItem.Quantity = cartItemPutDTO.Quantity;

            await cartRepository.UpdateAsync(cart);
            return mapper.Map<CartGetDTO>(cart);
        }

        public async Task DeleteItemFromCartAsync(int userId, int itemId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            var cartItemToRemove = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);

            if (cartItemToRemove == null)
            {
                throw new Exception("Cart item not found");
            }

            cart.CartItems.Remove(cartItemToRemove);
            await cartRepository.UpdateAsync(cart);
        }

        public async Task DeleteEntireCartAsync(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            cart.CartItems.Clear();
            await cartRepository.UpdateAsync(cart);
        }
    }
}
