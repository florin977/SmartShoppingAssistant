using SmartShoppingAssistant.BusinessLogic.Helpers.Strategies;
using SmartShoppingAssistant.BusinessLogic.PromotionLogic.Interfaces;
using SmartShoppingAssistant.BusinessLogic.PromotionLogic.Strategies;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic
{
    public static class PromotionStrategyFactory
    {
        public static IPromotionCondition GetConditionStrategy(PromotionType type)
        {
            return type switch
            {
                PromotionType.Quantity => new QuantityCondition(),
                PromotionType.CartTotal => new CartTotalCondition(),
                _ => throw new NotImplementedException($"Condition {type} is not supported.")
            };
        }

        public static IPromotionReward GetRewardStrategy(PromotionReward reward)
        {
            return reward switch
            {
                PromotionReward.PercentDiscount => new PercentDiscountReward(),
                PromotionReward.FreeItems => new FreeItemsReward(),
                _ => throw new NotImplementedException($"Reward {reward} is not supported.")
            };
        }
    }
}