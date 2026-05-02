using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController(IPromotionService promotionService) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<PromotionGetDTO>> AddPromotion(PromotionPostDTO promotionPostDTO)
        {
            try
            {
                var addedPromotion = await promotionService.AddAsync(promotionPostDTO);
                return Ok(addedPromotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the promotion: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> GetPromotionById(int id)
        {
            try
            {
                var promotion = await promotionService.GetByIdAsync(id);
                if (promotion == null)
                {
                    return NotFound();
                }
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the promotion: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionGetDTO>>> GetAllPromotions()
        {
            try
            {
                var promotions = await promotionService.GetAllAsync();
                return Ok(promotions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving promotions: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> UpdatePromotion(int id, PromotionPutDTO promotionPutDTO)
        {
            try
            {
                var updatedPromotion = await promotionService.UpdateAsync(id, promotionPutDTO);
                if (updatedPromotion == null)
                {
                    return NotFound();
                }
                return Ok(updatedPromotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the promotion: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromotion(int id)
        {
            try
            {
                await promotionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the promotion: {ex.Message}");
            }
        }
    }
}