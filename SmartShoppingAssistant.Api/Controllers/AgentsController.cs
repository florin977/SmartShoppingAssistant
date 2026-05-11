using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.Agents;
using SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;
using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentsController(ICartService cartService, ICategoryService categoryService, IPromotionCheckerAgent promotionCheckerAgent, ISuggestionComposerAgent suggestionComposerAgent, IMapper mapper) : ControllerBase
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
            var agent1 = promotionCheckerAgent.Build(cartJSON);
            var response1 = await agent1.RunAsync("Analyze the cart for current promotions");

            // Free API...so we wait a bit to avoid hitting rate limits.
            await Task.Delay(4000);

            var promotionAnalysisJSON = response1.Text;

            // Get all or get all that are in the cart ?
            var categories = await categoryService.GetAllAsync();
            var categorySlims = mapper.Map<List<CategorySlimGetDTO>>(categories);

            var categoriesJSON = JsonSerializer.Serialize(categorySlims);

            var agent2 = suggestionComposerAgent.Build(cartJSON, promotionAnalysisJSON, categoriesJSON);
            var response2 = await agent2.RunAsync("Generate product suggestions based on the cart and near-miss promotions.");
            var suggestionsJSON = response2.Text;

            try
            {
                var analysisResult = JsonSerializer.Deserialize<PromotionAnalysis>(promotionAnalysisJSON);
                var suggestionsResult = JsonSerializer.Deserialize<SuggestionResult>(suggestionsJSON);
                return Ok(new
                {
                    Promotions = analysisResult,
                    Suggestions = suggestionsResult
                });
            }
            catch (JsonException)
            {
                return Ok(new 
                {
                    RawPromotions = promotionAnalysisJSON,
                    RawSuggestions = suggestionsJSON
                });
            }
        }
    }
}
