using SmartShoppingAssistant.BusinessLogic.PromotionLogic;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Helpers.Strategies
{
    public interface IPromotionCondition
    {
        bool IsEligible(Promotion promotion, CartContext context);
    }
}