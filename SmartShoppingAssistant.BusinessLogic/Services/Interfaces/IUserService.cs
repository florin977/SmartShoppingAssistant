using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserGetDTO> GetByIdAsync(int id);
        Task<UserGetDTO> RegisterAsync(UserPostDTO userPostDTO);
        Task<string> LoginAsync(UserLoginDTO userLoginDTO);
        Task DeleteAsync(int id);
    }
}
