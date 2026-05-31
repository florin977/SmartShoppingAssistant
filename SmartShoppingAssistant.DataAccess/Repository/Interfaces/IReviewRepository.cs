using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
        Task<List<Review>> GetReviewsByUserIdAsync(int userId);
    }
}
