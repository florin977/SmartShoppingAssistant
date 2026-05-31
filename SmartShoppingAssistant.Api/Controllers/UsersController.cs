using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserGetDTO>> GetCurrentUser()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID claim is missing or invalid." });
            }

            try
            {
                var userDto = await userService.GetByIdAsync(userId.Value);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                // Valid token, but invalid user ID claim
                return Unauthorized(new { message = "Invalid user ID claim." });
            }

            bool isAdmin = User.IsInRole("Admin");
            bool isOwnAccount = userId.Value == id;

            if (!isAdmin && !isOwnAccount)
            {
                return Forbid();
            }

            try
            {
                await userService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}