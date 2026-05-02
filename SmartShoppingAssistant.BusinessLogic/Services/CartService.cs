using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartService(ICartRepository cartRepository, IMapper mapper) : ICartService
    {
        public async Task<CartGetDTO> GetCartAsync(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            return mapper.Map<CartGetDTO>(cart);
        }

        public async Task<CartGetDTO> AddItemToCartAsync(int userId, CartItemPostDTO cartItemPostDTO)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);

            // TODO: Remove after implementing User authentication and authorization
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
