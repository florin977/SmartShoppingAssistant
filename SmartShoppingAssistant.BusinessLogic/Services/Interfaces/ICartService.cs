using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartGetDTO> GetCartByUserIdAsync(int userId);
        Task<CartGetDTO> AddItemToCartAsync(int userId, CartItemPostDTO cartItemPostDTO);
        Task<CartGetDTO> UpdateItemFromCartAsync(int userId, int itemId, CartItemPutDTO cartItemPutDTO);
        Task DeleteItemFromCartAsync(int userId, int itemId);
        Task DeleteEntireCartAsync(int userId);
        Task<AnalysisResponse> AnalyzeCartWithAI(int userId);
    }
}
