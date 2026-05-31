using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<UserGetDTO> GetByIdAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return mapper.Map<UserGetDTO>(user);
        }
        public async Task DeleteAsync(int id)
        {
            await userRepository.DeleteAsync(id);
        }
    }
}
