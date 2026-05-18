using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.PromotionLogic.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic.Strategies
{
    public class FreeItemsReward : IPromotionReward
    {
        public decimal CalculateSavings(Promotion p, CartContext context)
        {
            var cheapest = context.GetCheapestApplicableItem(p);
            return cheapest != null ? p.RewardValue * cheapest.Product.Price : 0;
        }

        public void ApplyReward(Promotion p, CartContext context, decimal savings)
        {
            var cheapest = context.GetCheapestApplicableItem(p);
            if (cheapest == null) return;

            int freeQuantity = (int)p.RewardValue;

            // Check if there is already a free gift entry for this product
            var existingFreeItem = context.Items.FirstOrDefault(i =>
                i.Product.Id == cheapest.Product.Id && i.IsFreeGift);

            if (existingFreeItem != null)
            {
                existingFreeItem.Quantity += freeQuantity;
            }
            else
            {
                context.Items.Add(new CartItem
                {
                    Product = cheapest.Product,
                    ProductId = cheapest.Product.Id,
                    Quantity = freeQuantity,
                    IsFreeGift = true // Helps the frontend distinguish free items from regular ones
                });
            }

            context.TotalSavedOverall += savings;

            context.AppliedPromotions.Add(new CartAppliedPromotionDTO
            {
                PromotionName = p.Name,
                Discount = -savings
            });
        }
    }
}