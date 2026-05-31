using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class ReviewRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Review>(context), IReviewRepository
    {

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await context.Set<Review>().Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Likes) // TODO: maybe add more filters like rating, likes, etc.
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await context.Set<Review>().Where(r => r.UserId == userId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.Likes)
                .ToListAsync();
        }
    }
}
