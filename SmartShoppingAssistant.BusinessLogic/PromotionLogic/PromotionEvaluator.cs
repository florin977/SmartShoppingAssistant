using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.PromotionLogic;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic
{
    public static class PromotionEvaluator
    {
        public static void ApplyBestPromotion(
            ICollection<CartItem> cartItems,
            List<Promotion> activePromotions,
            List<CartAppliedPromotionDTO> appliedPromotionsList,
            ref decimal runningTotal,
            ref decimal totalSavedOverall)
        {
            var context = new CartContext(cartItems, appliedPromotionsList, runningTotal, totalSavedOverall);

            var bestPromoResult = activePromotions
                .Select(p => new
                {
                    Promotion = p,
                    Condition = PromotionStrategyFactory.GetConditionStrategy(p.Type),
                    Reward = PromotionStrategyFactory.GetRewardStrategy(p.Reward)
                })
                // Check Eligibility
                .Where(x => x.Condition.IsEligible(x.Promotion, context))
                // Calculate Potential Savings
                .Select(x => new
                {
                    x.Promotion,
                    x.Reward,
                    Savings = x.Reward.CalculateSavings(x.Promotion, context)
                })
                .Where(x => x.Savings > 0)
                // Sort to find the best one
                .OrderByDescending(x => x.Savings)
                .FirstOrDefault();

            if (bestPromoResult == null) return;

            // Apply the best promotion's reward
            bestPromoResult.Reward.ApplyReward(bestPromoResult.Promotion, context, bestPromoResult.Savings);

            // Sync ref variables back to caller
            runningTotal = context.RunningTotal;
            totalSavedOverall = context.TotalSavedOverall;
        }
    }
}