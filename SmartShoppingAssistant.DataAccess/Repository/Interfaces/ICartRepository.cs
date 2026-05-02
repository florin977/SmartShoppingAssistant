using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
    }
}
