using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Tools
{
    public static class ShoppingTools
    {
        [Description("Get all promotions for a specific product")]
        public static async Task<List<PromotionGetDTO>> GetPromotionsForProduct(IPromotionService promotionService, int productId)
        {
            return await promotionService.GetForProductAsync(productId);
        }
    }
}
