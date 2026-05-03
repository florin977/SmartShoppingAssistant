using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
