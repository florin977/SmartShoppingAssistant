using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class PromotionRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Promotion>(context), IPromotionRepository
    {
        public async Task<Promotion> GetActivePromotionByIdAsync(int id)
        {
            var promotion = await context.Promotions
                .Where(p => p.Id == id && p.IsActive)
                .FirstOrDefaultAsync();

            if (promotion == null)
            {
                throw new KeyNotFoundException($"No active promotion found with that ID");
            }

            return promotion;
        }
        public async Task<IEnumerable<Promotion>> GetAllActivePromotionsAsync()
        {
            var promotions = await context.Promotions
                .Where(p => p.IsActive)
                .ToListAsync();

            return promotions;
        }
        public async Task<List<Promotion>> GetForProductAsync(int productId)
        {
            var categoryIds = await context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Categories.Select(c => c.Id))
                .ToListAsync();

            return await GetAllAsQueryable()
                .Where(p => p.IsActive ||
                (p.CategoryId.HasValue && categoryIds.Contains(p.CategoryId.Value)))
                .ToListAsync();
        }
        public async Task<IEnumerable<Promotion>> GetActivePromotionsForProductsAsync(IEnumerable<int> productIds)
        {
            var promotions = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .SelectMany(pr => pr.Promotions.Where(promo => promo.IsActive))
                .Distinct()
                .ToListAsync();

            return promotions;
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsForCategoriesAsync(IEnumerable<int> categoryIds)
        {
            var promotions = await context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .SelectMany(c => c.Promotions.Where(pr => pr.IsActive))
                .Distinct()
                .ToListAsync();

            return promotions;
        }
        public async Task<IEnumerable<Promotion>> GetActivePromotionsForCartAsync()
        {
            var promotions = await context.Promotions
                .Where(p => p.IsActive && !p.ProductId.HasValue && !p.CategoryId.HasValue)
                .ToListAsync();

            return promotions;
        }
    }
}