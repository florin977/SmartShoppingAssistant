using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<PromotionGetDTO> AddAsync(PromotionPostDTO promotionPostDTO);
        Task<PromotionGetDTO> GetByIdAsync(int id);
        Task<IEnumerable<PromotionGetDTO>> GetAllAsync();
        Task<PromotionGetDTO> UpdateAsync(int id, PromotionPutDTO promotionPutDTO);
        Task DeleteAsync(int id);
        Task<PromotionGetDTO> GetActivePromotionByIdAsync(int id);
        Task<IEnumerable<PromotionGetDTO>> GetAllActivePromotionsAsync();
    }
}
