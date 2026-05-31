using AutoMapper;
using GenerativeAI.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class AuthService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IMapper mapper) : IAuthService
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
                await LogoutAllDevicesAsync(existingToken.UserId); // Revoke all tokens for the user if a revoked token is used
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

        public async Task<UserGetDTO> RegisterAsync(UserPostDTO userPostDTO)
        {
            if (await userRepository.ExistsByUsernameAsync(userPostDTO.Username))
            {
                throw new Exception("User already exists.");
            }

            if (await userRepository.ExistsByEmailAsync(userPostDTO.Email))
            {
                throw new Exception("User already exists.");
            }

            var user = mapper.Map<User>(userPostDTO);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userPostDTO.Password);
            // TODO: Assign role based on registration type (e.g., customer, admin)
            user.Role = UserRole.Customer;
            user.CreatedAt = DateTime.UtcNow;

            var createdUser = await userRepository.AddAsync(user);
            return mapper.Map<UserGetDTO>(createdUser);
        }

        public async Task<(string JwtToken, string RefreshToken)> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await userRepository.GetByEmailAsync(userLoginDTO.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (user.LockedOutUntil.HasValue && user.LockedOutUntil.Value > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Account is locked. Please try again later.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.PasswordHash))
            {
                user.LoginAttempts++;

                if (user.LoginAttempts >= 5)
                {
                    user.LockedOutUntil = DateTime.UtcNow.AddMinutes(15);
                    user.LoginAttempts = 0; // Reset attempts after locking out
                }

                await userRepository.UpdateAsync(user);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Reset login attempts on successful login
            user.LoginAttempts = 0;
            user.LockedOutUntil = null;
            await userRepository.UpdateAsync(user);

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            double refreshTokenLifetime = double.Parse(configuration["RefreshToken:ExpiresInDays"]!);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenLifetime) // Refresh token valid for the specified number of days
            };

            await refreshTokenRepository.AddAsync(refreshTokenEntity);

            // Return the tokens as a tuple. The controller will handle the cookie.
            return (jwtToken, refreshToken);
        }
        public async Task LogoutDeviceAsync(string refreshToken)
        {
            var refreshTokenEntity = await refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (refreshTokenEntity != null)
            {
                refreshTokenEntity.RevokedAt = DateTime.UtcNow;
                await refreshTokenRepository.UpdateAsync(refreshTokenEntity);
            }
        }
        public async Task LogoutAllDevicesAsync(int userId)
        {
            await refreshTokenRepository.RevokeAllUserTokensAsync(userId);
        }
    }
}