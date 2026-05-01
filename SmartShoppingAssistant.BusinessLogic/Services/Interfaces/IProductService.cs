using SmartShoppingAssistant.BusinessLogic.DTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductGetDTO>> GetAllAsync();
        Task<ProductGetDTO> AddAsync(ProductPostDTO productAddDTO);
        Task<ProductGetDTO> UpdateAsync(int id, ProductPutDTO productUpdateDTO);
        Task DeleteAsync(int id);
    }
}