using SmartShoppingAssistant.BusinessLogic.Helpers.Strategies;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic.Strategies
{
    public class CartTotalCondition : IPromotionCondition
    {
        public bool IsEligible(Promotion p, CartContext context)
        {
            return context.GetApplicableTotal(p) >= p.Threshold;
        }
    }
}