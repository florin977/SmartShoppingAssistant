using Microsoft.AspNetCore.Mvc;
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
                    Expires = DateTime.UtcNow.AddDays(30)
                };

                Response.Cookies.Append("jwtToken", newJwtToken, jwtCookieOptions);
                Response.Cookies.Append("refreshToken", newRefreshToken, refreshCookieOptions);

                return Ok(new { message = "Token refreshed successfully." });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}