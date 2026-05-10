using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Helpers
{
    internal static class PromotionEvaluator
    {
        public static void ApplyBestPromotion(
            ICollection<CartItem> cartItems,
            List<Promotion> activePromotions,
            List<CartAppliedPromotionDTO> appliedPromotionsList,
            ref decimal runningTotal,
            ref decimal totalSavedOverall)
        {
            var (byProduct, byCategory) = ComputeCartAggregates(cartItems);

            decimal currentRunningTotal = runningTotal;

            var bestPromotionData = activePromotions
                .Where(p => IsEligible(p, byProduct, byCategory, currentRunningTotal))
                .Select(p =>
                {
                    var result = ComputePromoResult(p, cartItems, byProduct, byCategory, currentRunningTotal);
                    return new { Promotion = p, result.TotalSaved, result.FreeItemQuantity, result.FreeItemProductId };
                })
                .OrderByDescending(r => r.TotalSaved)
                .FirstOrDefault();

            if (bestPromotionData == null || (bestPromotionData.TotalSaved == 0 && bestPromotionData.FreeItemQuantity == 0))
                return;

            appliedPromotionsList.Add(new CartAppliedPromotionDTO
            {
                PromotionName = bestPromotionData.Promotion.Name,
                Discount = bestPromotionData.TotalSaved * -1
            });

            totalSavedOverall += bestPromotionData.TotalSaved;

            if (bestPromotionData.Promotion.Reward == PromotionReward.FreeItems && bestPromotionData.FreeItemProductId.HasValue)
            {
                var freeItem = cartItems.FirstOrDefault(ci => ci.Product.Id == bestPromotionData.FreeItemProductId.Value);
                if (freeItem != null)
                {
                    freeItem.Quantity += bestPromotionData.FreeItemQuantity;
                }
            }
            else if (bestPromotionData.Promotion.Reward == PromotionReward.PercentDiscount)
            {
                runningTotal -= bestPromotionData.TotalSaved;
            }
        }

        private static (Dictionary<int, (int Quantity, decimal Total)> byProduct,
                        Dictionary<int, (int Quantity, decimal Total)> byCategory)
            ComputeCartAggregates(IEnumerable<CartItem> items)
        {
            var byProduct = items
                .GroupBy(i => i.Product.Id)
                .ToDictionary(g => g.Key, g => (Quantity: g.Sum(i => i.Quantity), Total: g.Sum(i => i.Quantity * i.Product.Price)));

            var byCategory = items
                .SelectMany(i => i.Product.Categories, (i, c) => (Category: c, Item: i))
                .GroupBy(x => x.Category.Id)
                .ToDictionary(g => g.Key, g => (Quantity: g.Sum(x => x.Item.Quantity), Total: g.Sum(x => x.Item.Quantity * x.Item.Product.Price)));

            return (byProduct, byCategory);
        }

        private static decimal GetApplicableTotal(Promotion p, Dictionary<int, (int Quantity, decimal Total)> byProduct, Dictionary<int, (int Quantity, decimal Total)> byCategory, decimal currentRunningTotal)
        {
            if (p.ProductId.HasValue) return byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Total : 0;
            if (p.CategoryId.HasValue) return byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Total : 0;
            return currentRunningTotal;
        }

        private static int GetApplicableQuantity(Promotion p, Dictionary<int, (int Quantity, decimal Total)> byProduct, Dictionary<int, (int Quantity, decimal Total)> byCategory)
        {
            if (p.ProductId.HasValue) return byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Quantity : 0;
            if (p.CategoryId.HasValue) return byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Quantity : 0;
            return byProduct.Values.Sum(p => p.Quantity);
        }

        private static CartItem? GetCheapestItem(Promotion p, IEnumerable<CartItem> items)
        {
            if (p.ProductId.HasValue) return items.Where(i => i.Product.Id == p.ProductId.Value).OrderBy(i => i.Product.Price).FirstOrDefault();
            if (p.CategoryId.HasValue) return items.Where(i => i.Product.Categories.Any(c => c.Id == p.CategoryId.Value)).OrderBy(i => i.Product.Price).FirstOrDefault();
            return items.OrderBy(i => i.Product.Price).FirstOrDefault();
        }

        private static bool IsEligible(Promotion p, Dictionary<int, (int Quantity, decimal Total)> byProduct, Dictionary<int, (int Quantity, decimal Total)> byCategory, decimal currentRunningTotal)
        {
            return p.Type switch
            {
                PromotionType.Quantity => GetApplicableQuantity(p, byProduct, byCategory) >= p.Threshold,
                PromotionType.CartTotal => GetApplicableTotal(p, byProduct, byCategory, currentRunningTotal) >= p.Threshold,
                _ => false
            };
        }

        private static (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) ComputePromoResult(Promotion p, IEnumerable<CartItem> items, Dictionary<int, (int Quantity, decimal Total)> byProduct, Dictionary<int, (int Quantity, decimal Total)> byCategory, decimal currentRunningTotal)
        {
            return p.Reward switch
            {
                PromotionReward.PercentDiscount => (GetApplicableTotal(p, byProduct, byCategory, currentRunningTotal) * (p.RewardValue / 100m), 0, null),
                PromotionReward.FreeItems => GetCheapestItem(p, items) is { } cheapest ? (p.RewardValue * cheapest.Product.Price, (int)p.RewardValue, cheapest.Product.Id) : (0, 0, null),
                _ => (0, 0, null)
            };
        }
    }
}