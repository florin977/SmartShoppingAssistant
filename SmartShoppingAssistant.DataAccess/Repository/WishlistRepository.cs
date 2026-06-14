using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using SmartShoppingAssistant.DataAccess.Parameters;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class WishlistRepository(SmartShoppingAssistantDbContext context) : BaseRepository<WishlistItem>(context), IWishlistRepository
    {
        public async Task<PagedResult<WishlistItem>> GetByUserIdAsync(int userId, PaginationParameters paginationParameters)
        {
            var query = context.Set<WishlistItem>().AsQueryable()
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .OrderByDescending(w => w.AddedAt);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParameters.PageSize);

            var items = await query.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new PagedResult<WishlistItem>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<WishlistItem?> GetByUserAndProductAsync(int userId, int productId)
        {
            return await context.Set<WishlistItem>().FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public override async Task<WishlistItem> GetByIdAsync(int id)
        {
            return await context.Set<WishlistItem>()
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
