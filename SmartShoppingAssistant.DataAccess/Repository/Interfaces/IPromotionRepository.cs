using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<Promotion> GetActivePromotionByIdAsync(int id);
        Task<IEnumerable<Promotion>> GetAllActivePromotionsAsync();
    }
}
