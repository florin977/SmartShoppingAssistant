using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic.Interfaces
{
    public interface IPromotionReward
    {
        decimal CalculateSavings(Promotion promotion, CartContext context);
        void ApplyReward(Promotion promotion, CartContext context, decimal savings);
    }
}