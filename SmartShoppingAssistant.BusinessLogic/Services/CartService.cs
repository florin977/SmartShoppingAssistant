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
    public class CartService(ICartRepository cartRepository, IMapper mapper) : ICartService
    {
        public async Task<CartGetDTO> GetCartByUserIdAsync(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            var cartDTO = mapper.Map<CartGetDTO>(cart);
            cartDTO.TotalBeforeDiscount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) = PromotionEvaluator.GetBestPromotion(cartDTO.CartItems, cartDTO.TotalBeforeDiscount);
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
