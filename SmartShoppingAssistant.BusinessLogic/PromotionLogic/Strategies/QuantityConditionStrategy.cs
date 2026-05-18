using SmartShoppingAssistant.BusinessLogic.Helpers.Strategies;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic.Strategies
{
    public class QuantityCondition : IPromotionCondition
    {
        public bool IsEligible(Promotion p, CartContext context)
        {
            return context.GetApplicableQuantity(p) >= p.Threshold;
        }
    }
}