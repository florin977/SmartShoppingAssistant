using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Parameters;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<PagedResult<Review>> GetReviewsByProductIdAsync(int productId, PaginationParameters paginationParameters);
        Task<PagedResult<Review>> GetReviewsByUserIdAsync(int userId, PaginationParameters paginationParameters);
    }
}
