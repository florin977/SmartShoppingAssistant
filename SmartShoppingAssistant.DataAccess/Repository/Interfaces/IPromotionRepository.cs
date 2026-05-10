using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<Promotion> GetActivePromotionByIdAsync(int id);
        Task<IEnumerable<Promotion>> GetAllActivePromotionsAsync();
        Task<List<Promotion>> GetForProductAsync(int productId);
        Task<IEnumerable<Promotion>> GetActivePromotionsForProductsAsync(IEnumerable<int> productIds);
        Task<IEnumerable<Promotion>> GetActivePromotionsForCategoriesAsync(IEnumerable<int> categoryIds);
        Task<IEnumerable<Promotion>> GetActivePromotionsForCartAsync();
    }
}
