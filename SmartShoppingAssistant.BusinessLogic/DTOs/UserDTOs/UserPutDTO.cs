namespace SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs
{
    // No put DTO for user, will need separate endpoints for updating email, password, and role. This is because of security reasons, we don't want to allow updating all fields at once.
    internal class UserPutDTO
    {
    }
}
