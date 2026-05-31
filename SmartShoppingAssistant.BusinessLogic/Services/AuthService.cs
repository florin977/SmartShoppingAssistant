using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class AuthService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository) : IAuthService
    {
        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: credentials
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<(string JwtToken, string RefreshToken)> RefreshTokensAsync(string refreshToken)
        {
            var existingToken = await refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken == null || existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            if (existingToken.RevokedAt != null)
            {
                await refreshTokenRepository.RevokeAllUserTokensAsync(existingToken.UserId);// Revoke all tokens for the user
                throw new UnauthorizedAccessException("Refresh token has been revoked.");
            }

            var user = await userRepository.GetByIdAsync(existingToken.UserId);
            var newJwtToken = GenerateJwtToken(user);   
            var newRefreshToken = GenerateRefreshToken();

            double refreshTokenLifetime = double.Parse(configuration["RefreshToken:ExpiresInDays"]!);

            RefreshToken newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenLifetime) // Set new expiration date
            };

            existingToken.RevokedAt = DateTime.UtcNow; // Revoke the old token

            await refreshTokenRepository.UpdateAsync(existingToken);
            await refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            return (newJwtToken, newRefreshToken);
        }
    }
}
