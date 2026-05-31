using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class ReviewRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Review>(context), IReviewRepository
    {

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await context.Set<Review>().Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Likes) // TODO: maybe add more filters like rating, likes, etc.
                .ToListAsync();

            if (reviews == null)
            {
                throw new Exception($"No reviews found for product with id {productId}");
            }

            return reviews;
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            var reviews = await context.Set<Review>().Where(r => r.UserId == userId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.Likes)
                .ToListAsync();

            if (reviews == null)
            {
                throw new Exception($"No reviews found for user with id {userId}");
            }

            return reviews;
        }
    }
}
