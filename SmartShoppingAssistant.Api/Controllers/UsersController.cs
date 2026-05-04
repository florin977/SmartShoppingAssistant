using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.Security.Claims;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserGetDTO>> Register(UserPostDTO userPostDTO)
        {
            try
            {
                var registeredUser = await userService.RegisterAsync(userPostDTO);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var token = await userService.LoginAsync(userLoginDTO);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userIdClaimString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaimString, out int userIdClaim))
            {
                // Valid token, but invalid user ID claim
                return Unauthorized(new { message = "Invalid user ID claim." });
            }

            bool isAdmin = User.IsInRole("Admin");
            bool isOwnAccount = userIdClaim == id;

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