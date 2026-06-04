using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.DataAccess.Parameters;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductGetDTO>> GetAllAsync();
        Task<PagedResult<ProductGetDTO>> GetFilteredAsync(ProductQueryDTO productQuery);
        Task<ProductGetDTO> AddAsync(ProductPostDTO productAddDTO);
        Task<ProductGetDTO> UpdateAsync(int id, ProductPutDTO productUpdateDTO);
        Task DeleteAsync(int id);
    }
}