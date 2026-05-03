using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.Security;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs
{
    public class UserGetDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
