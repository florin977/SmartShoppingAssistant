using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using Microsoft.Extensions.Configuration;
using SmartShoppingAssistant.BusinessLogic.Helpers;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration) : IUserService
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
        public async Task<string> LoginAsync(UserLoginDTO userLoginDTO)
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

            // Return the token as a string. The controller will handle the cookie.
            return TokenGeneration.GenerateJwtToken(user, configuration);
        }
        public async Task DeleteAsync(int id)
        {
            await userRepository.DeleteAsync(id);
        }
    }
}
