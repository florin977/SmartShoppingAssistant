using Microsoft.Extensions.Configuration;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        Task<(string JwtToken, string RefreshToken)> RefreshTokensAsync(string refreshToken);
    }
}
