using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class CartRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Cart>(context), ICartRepository
    {
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            var cart = await context.Set<Cart>()
                 .Include(c => c.CartItems)
                     .ThenInclude(ci => ci.Product)
                 .FirstOrDefaultAsync(c => c.UserId == userId);

            // Ignore the warning, we will handle the case where the cart is null in the service layer, and create a new cart if it doesn't exist
            return cart;
        }
    }
}
