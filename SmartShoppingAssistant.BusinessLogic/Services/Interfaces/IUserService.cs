using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserGetDTO> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
