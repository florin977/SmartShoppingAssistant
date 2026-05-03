using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class UserRepository(SmartShoppingAssistantDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User> AddAsync(User user)
        {
            user.Cart = new Cart();
            return await base.AddAsync(user); // Throws on failure
        }
        // Let's hope delete is set to cascade in the database, otherwise we will have to delete the cart and refresh tokens manually
    }
}
