using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;
using SmartShoppingAssistant.DataAccess.Parameters;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IWishlistRepository : IRepository<WishlistItem>
    {
        Task<PagedResult<WishlistItem>> GetByUserIdAsync(int userId, PaginationParameters paginationParameters);
        Task<WishlistItem?> GetByUserAndProductAsync(int userId, int productId);
    }
}
