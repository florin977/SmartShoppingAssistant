using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId, [FromQuery] PaginationQueryDTO paginationQuery)
        {
            var reviews = await reviewService.GetReviewsByProductIdAsync(productId, paginationQuery);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser([FromRoute] int userId, [FromQuery] PaginationQueryDTO paginationQuery)
        {
            var reviews = await reviewService.GetReviewsByUserIdAsync(userId, paginationQuery);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(ReviewPostDTO reviewPostDTO)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var review = await reviewService.AddReviewAsync(reviewPostDTO, userId.Value);

            if (review == null)
            {
                return BadRequest(new { message = "Failed to add review." });
            }

            return Ok(review);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, ReviewPutDTO reviewPutDTO)
        {
            // Admin should only delete the review, not update it. So we only check if the user is the owner of the review.
            var userId = User.GetUserId();

            var review = await reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound(new { message = $"Review with ID {id} not found." });
            }

            if (review.User.Id != userId)
            {
                return Forbid();
            }

            var updatedReview = await reviewService.UpdateReviewAsync(id, reviewPutDTO);
            return Ok(updatedReview);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromRoute] int id)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var review = await reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound(new { message = $"Review with ID {id} not found." });
            }

            if (!isAdmin && review.User.Id != userId)
            {
                return Forbid();
            }

            await reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
}
