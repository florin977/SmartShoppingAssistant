using AutoMapper;
using Microsoft.Extensions.Configuration;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class UserService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IMapper mapper, IAuthService authService, IConfiguration configuration) : IUserService
    {
        public async Task<UserGetDTO> GetByIdAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return mapper.Map<UserGetDTO>(user);
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

            var jwtToken = authService.GenerateJwtToken(user);
            var refreshToken = authService.GenerateRefreshToken();

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
        public async Task DeleteAsync(int id)
        {
            await userRepository.DeleteAsync(id);
        }
    }
}
