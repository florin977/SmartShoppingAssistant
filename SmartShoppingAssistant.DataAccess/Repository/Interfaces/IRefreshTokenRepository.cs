using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByUserId(int userId);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task RevokeAllUserTokensAsync(int userId);
    }
}
