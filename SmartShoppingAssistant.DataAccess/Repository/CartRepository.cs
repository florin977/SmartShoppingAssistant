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

            // TODO: Maybe remove the comments ? Might need to create a cart on the fly if it doesn't exist
            /*
            if (cart == null)
            {
                throw new Exception($"Cart not found for queried user");
            }
            */

            return cart;
        }
    }
}
