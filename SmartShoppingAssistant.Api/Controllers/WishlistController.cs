using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.WishlistDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController(IWishlistService wishlistService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMine([FromQuery] PaginationQueryDTO paginationQuery)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var result = await wishlistService.GetByUserIdAsync(userId.Value, paginationQuery);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(WishlistItemPostDTO postDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var added = await wishlistService.AddToWishlistAsync(postDto, userId.Value);
            return Ok(added);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var item = await wishlistService.GetByIdAsync(id);
            if (item == null) return NotFound();
            if (item.UserId != userId) return Forbid();

            await wishlistService.DeleteFromWishlistAsync(id);
            return NoContent();
        }
    }
}
