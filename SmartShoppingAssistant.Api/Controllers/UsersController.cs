using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.Security.Claims;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, IConfiguration configuration) : ControllerBase
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
                var (jwtToken, refreshToken) = await userService.LoginAsync(userLoginDTO);

                double jwtLifespan = double.Parse(configuration["Jwt:ExpiresInMinutes"]!);
                double refreshTokenLifetime = double.Parse(configuration["RefreshToken:ExpiresInDays"]!);

                // Set the token as an HttpOnly cookie
                var jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(jwtLifespan)
                };

                var refreshCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(refreshTokenLifetime),
                    Path = "/api/auth/refresh" // Ensure the refresh token cookie is only sent to the refresh 
                };

                Response.Cookies.Append("jwtToken", jwtToken, jwtCookieOptions);
                Response.Cookies.Append("refreshToken", refreshToken, refreshCookieOptions);

                return Ok(new { message = "Login successful." });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
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