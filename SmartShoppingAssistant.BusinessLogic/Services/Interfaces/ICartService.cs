using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartGetDTO> GetCartAsync(int userId);
        Task<CartGetDTO> AddItemToCartAsync(int userId, CartItemPostDTO cartItemPostDTO);
        Task<CartGetDTO> UpdateItemFromCartAsync(int userId, int itemId, CartItemPutDTO cartItemPutDTO);
        Task DeleteItemFromCartAsync(int userId, int itemId);
        Task DeleteEntireCartAsync(int userId);
    }
}
