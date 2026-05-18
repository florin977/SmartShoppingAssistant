using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.PromotionLogic.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic.Strategies
{
    public class PercentDiscountReward : IPromotionReward
    {
        public decimal CalculateSavings(Promotion p, CartContext context)
        {
            return context.GetApplicableTotal(p) * (p.RewardValue / 100m);
        }

        public void ApplyReward(Promotion p, CartContext context, decimal savings)
        {
            context.RunningTotal -= savings;
            context.TotalSavedOverall += savings;

            context.AppliedPromotions.Add(new CartAppliedPromotionDTO
            {
                PromotionName = p.Name,
                Discount = -savings
            });
        }
    }
}