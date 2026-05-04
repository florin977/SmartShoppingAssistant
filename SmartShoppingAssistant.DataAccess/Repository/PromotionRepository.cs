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
    }
}
