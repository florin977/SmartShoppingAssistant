using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CategoryGetDTO>> Add(CategoryPostDTO categoryPostDTO)
        {
            try
            {
                var addedCategory = await categoryService.AddAsync(categoryPostDTO);
                return Ok(addedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryGetDTO>> GetById(int id)
        {
            try
            {
                var category = await categoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryGetDTO>>> GetAll()
        {
            try
            {
                var categories = await categoryService.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryGetDTO>> Update(int id, CategoryPutDTO categoryPutDTO)
        {
            try
            {
                var updatedCategory = await categoryService.UpdateAsync(id, categoryPutDTO);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
