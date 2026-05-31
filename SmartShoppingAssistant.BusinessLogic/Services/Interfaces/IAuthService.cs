using Microsoft.Extensions.Configuration;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        Task<(string JwtToken, string RefreshToken)> RefreshTokensAsync(string refreshToken);
        Task<UserGetDTO> RegisterAsync(UserPostDTO userPostDTO);
        Task<(string JwtToken, string RefreshToken)> LoginAsync(UserLoginDTO userLoginDTO);
        Task LogoutDeviceAsync(string refreshToken);
        Task LogoutAllDevicesAsync(int userId);
    }
}
