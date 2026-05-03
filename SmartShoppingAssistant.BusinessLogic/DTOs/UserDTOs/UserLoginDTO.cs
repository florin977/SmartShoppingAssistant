using System.Security;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
