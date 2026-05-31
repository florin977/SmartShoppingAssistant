using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("refresh")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(new { message = "Refresh token is missing." });
                }

                var (newJwtToken, newRefreshToken) = await authService.RefreshTokensAsync(refreshToken);

                double jwtLifespan = double.Parse(configuration["Jwt:ExpiresInMinutes"]!);

                // Set the new tokens as HttpOnly cookies
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
                    Expires = DateTime.UtcNow.AddDays(30),
                    Path = "/api/Auth" // Ensure the refresh token cookie is only sent to the Auth endpoints
                };

                Response.Cookies.Append("jwtToken", newJwtToken, jwtCookieOptions);
                Response.Cookies.Append("refreshToken", newRefreshToken, refreshCookieOptions);

                return Ok(new { message = "Token refreshed successfully." });
            }
            catch (Exception ex)
            {
                Response.Cookies.Delete("jwtToken");
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/Auth" });

                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserGetDTO>> Register(UserPostDTO userPostDTO)
        {
            try
            {
                var registeredUser = await authService.RegisterAsync(userPostDTO);
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
                var (jwtToken, refreshToken) = await authService.LoginAsync(userLoginDTO);

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
                    Path = "/api/Auth" // Ensure the refresh token cookie is only sent to the Auth endpoints 
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
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                return BadRequest(new { message = "Refresh token is missing." });
            }
            try
            {
                await authService.LogoutDeviceAsync(refreshToken);
                // Clear the cookies
                Response.Cookies.Delete("jwtToken");
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/Auth" });
                return Ok(new { message = "Logout successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("logoutAll")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID claim is missing or invalid." });
            }
            try
            {
                await authService.LogoutAllDevicesAsync(userId.Value);
                // Clear the cookies
                Response.Cookies.Delete("jwtToken");
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/Auth" });
                return Ok(new { message = "Logout from all devices successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}