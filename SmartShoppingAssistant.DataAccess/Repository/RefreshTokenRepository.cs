using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class RefreshTokenRepository(SmartShoppingAssistantDbContext context) : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken> GetByUserId(int userId)
        {
            try
            {
                var refreshToken = await context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == userId);

                if (refreshToken == null)
                {
                    throw new Exception($"No refresh token found for user with ID {userId}");
                }

                return refreshToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the refresh token for user {userId}: {ex.Message}", ex);
            }
        }
        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            try
            {
                var refreshToken = await context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token);
                
                if (refreshToken == null)
                {
                    throw new Exception("No refresh token found for the provided token");
                }
                
                return refreshToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the refresh token: {ex.Message}", ex);
            }
        }
        public async Task RevokeAllUserTokensAsync(int userId)
        {
            await context.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null) // Only update active ones
                .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.RevokedAt, DateTime.UtcNow));
        }
    }
}