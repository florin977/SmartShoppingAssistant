using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.Security;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs
{
    public class UserSummaryGetDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
