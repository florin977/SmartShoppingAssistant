using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class CartRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Cart>(context), ICartRepository
    {
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            // Ask about the nesting of the includes, is it too much ?
            var cart = await context.Set<Cart>()
                 .Include(c => c.CartItems)
                     .ThenInclude(ci => ci.Product)
                         .ThenInclude(p => p.Promotions)
                 .Include(c => c.CartItems)
                     .ThenInclude(ci => ci.Product)
                         .ThenInclude(p => p.Categories)
                             .ThenInclude(c => c.Promotions)
                 .FirstOrDefaultAsync(c => c.UserId == userId);

            // TODO: Maybe remove the comments ? Might need to create a cart on the fly if it doesn't exist
            /*
            if (cart == null)
            {
                throw new Exception($"Cart not found for queried user");
            }
            */

            // Ignore the warning, we will handle the case where the cart is null in the service layer, and create a new cart if it doesn't exist
            return cart;
        }
    }
}
