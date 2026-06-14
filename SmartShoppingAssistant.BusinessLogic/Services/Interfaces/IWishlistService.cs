using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.WishlistDTOs;
using SmartShoppingAssistant.DataAccess.Parameters;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<PagedResult<WishlistItemGetDTO>> GetByUserIdAsync(int userId, PaginationQueryDTO paginationQuery);
        Task<WishlistItemGetDTO?> AddToWishlistAsync(WishlistItemPostDTO postDto, int userId);
        Task DeleteFromWishlistAsync(int wishlistItemId);
        Task<WishlistItemGetDTO?> GetByIdAsync(int id);
        Task<WishlistItemGetDTO?> GetByUserAndProductAsync(int userId, int productId);
    }
}
