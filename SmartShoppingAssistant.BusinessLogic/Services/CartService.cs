using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.Helpers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartService(ICartRepository cartRepository, IPromotionRepository promotionRepository, IMapper mapper) : ICartService
    {
        public async Task<CartGetDTO> GetCartByUserIdAsync(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await cartRepository.AddAsync(cart);
            }

            var productIds = cart.CartItems.Select(ci => ci.ProductId).Distinct().ToList();
            var categoryIds = cart.CartItems.SelectMany(ci => ci.Product.Categories.Select(c => c.Id)).Distinct().ToList();

            var productPromotions = await promotionRepository.GetActivePromotionsForProductsAsync(productIds);
            var categoryPromotions = await promotionRepository.GetActivePromotionsForCategoriesAsync(categoryIds);
            var cartPromotions = await promotionRepository.GetActivePromotionsForCartAsync();

            decimal runningTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
            decimal totalSavedOverall = 0;
            var appliedPromotions = new List<CartAppliedPromotionDTO>();

            PromotionEvaluator.ApplyBestPromotion(cart.CartItems, productPromotions.ToList(), appliedPromotions, ref runningTotal, ref totalSavedOverall);
            PromotionEvaluator.ApplyBestPromotion(cart.CartItems, categoryPromotions.ToList(), appliedPromotions, ref runningTotal, ref totalSavedOverall);
            PromotionEvaluator.ApplyBestPromotion(cart.CartItems, cartPromotions.ToList(), appliedPromotions, ref runningTotal, ref totalSavedOverall);

            decimal finalSubtotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            var cartDTO = new CartGetDTO
            {
                Subtotal = finalSubtotal,
                TotalDiscount = totalSavedOverall * -1, // Negative discounts, as per convention
                Total = finalSubtotal - totalSavedOverall,
                AppliedPromotions = appliedPromotions,

                Items = cart.CartItems.Select(ci => new CartItemGetDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Quantity * ci.Product.Price
                }).ToList()
            };

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
