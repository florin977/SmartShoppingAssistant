using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.Security.Claims;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService, IMapper mapper) : ControllerBase
    {
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        [HttpPost("items")]
        public async Task<ActionResult> AddItemToCart([FromBody] CartItemPostDTO cartItemPostDTO)
        {
            try
            {
                var userId = GetUserId();
                
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID claim is missing or invalid." });
                }

                await cartService.AddItemToCartAsync(userId.Value, cartItemPostDTO);
                return Ok(new { message = "Item added to cart successfully." });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var inner = ex.InnerException?.Message;
                var innerInner = ex.InnerException?.InnerException?.Message;

                return BadRequest(new { message, inner, innerInner });
            }
        }
        [HttpGet]
        public async Task<ActionResult<CartGetDTO>> GetCart()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID claim is missing or invalid." });
                }
                var cart = await cartService.GetCartByUserIdAsync(userId.Value);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("items/{itemId}")]
        public async Task<ActionResult> UpdateCartItem(int itemId, CartItemPutDTO cartItemPutDTO)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID claim is missing or invalid." });
                }
                await cartService.UpdateItemFromCartAsync(userId.Value, itemId, cartItemPutDTO);
                return Ok(new { message = "Cart item updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult> DeleteCartItem(int itemId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID claim is missing or invalid." });
                }
                await cartService.DeleteItemFromCartAsync(userId.Value, itemId);
                return Ok(new { message = "Cart item deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteEntireCart()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID claim is missing or invalid." });
                }
                await cartService.DeleteEntireCartAsync(userId.Value);
                return Ok(new { message = "Cart deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeCartWithAI()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID claim is missing or invalid." });
            }

            var response = await cartService.AnalyzeCartWithAI(userId.Value);

            return Ok(response);
        }
    }
}