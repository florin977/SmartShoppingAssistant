using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.BusinessLogic.Agents;
using SmartShoppingAssistant.BusinessLogic.Models;
using System.Security.Claims;
using System.Text.Json;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentsController(ICartService cartService, IPromotionCheckerAgent promotionCheckerAgent) : ControllerBase
    {
        [HttpGet("analyze")]
        public async Task<IActionResult> AnalyzeCartWithAI()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User ID claim is missing or invalid." });
            }

            var cart = await cartService.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                return BadRequest("Your cart is empty. Add items to see promotion analysis.");
            }

            var cartJSON = JsonSerializer.Serialize(cart);
            var agent = promotionCheckerAgent.Build(cartJSON);
            var response = await agent.RunAsync("Analyze the cart for current promotions");

            try
            {
                var analysisResult = JsonSerializer.Deserialize<PromotionAnalysis>(response.Text);
                return Ok(analysisResult);
            }
            catch (JsonException)
            {
                return Ok(new { rawResponse = response.Text });
            }
        }
    }
}
