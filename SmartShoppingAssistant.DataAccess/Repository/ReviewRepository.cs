using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Parameters;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class ReviewRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Review>(context), IReviewRepository
    {

        public async Task<PagedResult<Review>> GetReviewsByProductIdAsync(int productId, PaginationParameters paginationParameters)
        {
            var query = context.Set<Review>().AsQueryable();

            query = query.Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Likes);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParameters.PageSize);

            var reviews = await query.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Review>
            {
                Items = reviews,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<PagedResult<Review>> GetReviewsByUserIdAsync(int userId, PaginationParameters paginationParameters)
        {
            var query = context.Set<Review>().AsQueryable();
            query = query.Where(r => r.UserId == userId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.Likes);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParameters.PageSize);

            var reviews = await query.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Review>
            {
                Items = reviews,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
        public async Task<Review> GetByProductAndUserId(int productId, int userId)
        {
            return await context.Set<Review>().FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        }

        public override async Task<Review> GetByIdAsync(int id)
        {
            return await context.Set<Review>()
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
