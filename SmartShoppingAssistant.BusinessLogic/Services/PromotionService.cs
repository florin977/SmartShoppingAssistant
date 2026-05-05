using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class PromotionService(IPromotionRepository promotionRepository, IMapper mapper) : IPromotionService
    {
        public async Task<PromotionGetDTO> AddAsync(PromotionPostDTO promotionPostDTO)
        {
            var promotion = mapper.Map<Promotion>(promotionPostDTO);
            var createdPromotion = await promotionRepository.AddAsync(promotion);
            return mapper.Map<PromotionGetDTO>(createdPromotion);
        }
        public async Task<PromotionGetDTO> GetByIdAsync(int id)
        {
            var queriedPromotion = await promotionRepository.GetByIdAsync(id);
            return mapper.Map<PromotionGetDTO>(queriedPromotion);
        }
        public async Task<IEnumerable<PromotionGetDTO>> GetAllAsync()
        {
            var queriedPromotions = await promotionRepository.GetAllAsync();
            return mapper.Map<List<PromotionGetDTO>>(queriedPromotions);
        }
        public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionPutDTO promotionPutDTO)
        {
            var promotionToUpdate = await promotionRepository.GetByIdAsync(id);
            mapper.Map(promotionPutDTO, promotionToUpdate);
            var updatedPromotion = await promotionRepository.UpdateAsync(promotionToUpdate);
            return mapper.Map<PromotionGetDTO>(updatedPromotion);
        }
        public async Task DeleteAsync(int id)
        {
            await promotionRepository.DeleteAsync(id);
        }

        public async Task<PromotionGetDTO> GetActivePromotionByIdAsync(int id)
        {
            var activePromotion = await promotionRepository.GetActivePromotionByIdAsync(id);
            return mapper.Map<PromotionGetDTO>(activePromotion);
        }
        public async Task<IEnumerable<PromotionGetDTO>> GetAllActivePromotionsAsync()
        {
            var activePromotions = await promotionRepository.GetAllActivePromotionsAsync();
            return mapper.Map<List<PromotionGetDTO>>(activePromotions);
        }
        public async Task<List<PromotionGetDTO>> GetForProductAsync(int productId)
        {
            var promotionsForProduct = await promotionRepository.GetForProductAsync(productId);
            return mapper.Map<List<PromotionGetDTO>>(promotionsForProduct);
        }
    }
}