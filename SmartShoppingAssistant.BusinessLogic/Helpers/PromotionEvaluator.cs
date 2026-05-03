using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Helpers
{
    internal static class PromotionEvaluator
    {
        private static (Dictionary<int, (int Quantity, decimal Total)> byProduct,
                        Dictionary<int, (int Quantity, decimal Total)> byCategory)
            ComputeCartAggregates(IEnumerable<CartItemGetDTO> items)
        {
            var byProduct = items
                .GroupBy(i => i.Product.Id)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        Quantity: g.Sum(i => i.Quantity),
                        Total: g.Sum(i => i.Quantity * i.Product.Price)
                    )
                );

            var byCategory = items
                .SelectMany(i => i.Product.Categories, (i, c) => (Category: c, Item: i))
                .GroupBy(x => x.Category.Id)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        Quantity: g.Sum(x => x.Item.Quantity),
                        Total: g.Sum(x => x.Item.Quantity * x.Item.Product.Price)
                    )
                );

            return (byProduct, byCategory);
        }

        // Only active promotions are returned from the repository, so no need to check IsActive here.
        private static IEnumerable<PromotionGetDTO> GetAllPromotions(CartItemGetDTO item)
        {
            var productPromotions = item.Product.Promotions ?? Enumerable.Empty<PromotionGetDTO>();

            var categoryPromotions = item.Product.Categories
                .SelectMany(c => c.Promotions ?? Enumerable.Empty<PromotionGetDTO>());

            return productPromotions
                .Concat(categoryPromotions)
                .DistinctBy(p => p.Id);
        }

        private static bool IsEligible(
            PromotionGetDTO p,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            return p.Type switch
            {
                PromotionType.Quantity =>
                    p.ProductId.HasValue
                        ? byProduct.TryGetValue(p.ProductId.Value, out var prod) && prod.Quantity >= p.Threshold
                        : p.CategoryId.HasValue
                            ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) && cat.Quantity >= p.Threshold
                            : false,

                PromotionType.CartTotal =>
                    p.ProductId.HasValue
                        ? byProduct.TryGetValue(p.ProductId.Value, out var prod) && prod.Total >= p.Threshold
                        : p.CategoryId.HasValue
                            ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) && cat.Total >= p.Threshold
                            : cartTotal >= p.Threshold,

                _ => false
            };
        }

        private static IEnumerable<PromotionGetDTO> GetEligiblePromotions(
            IEnumerable<CartItemGetDTO> items,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            return items
                .SelectMany(GetAllPromotions)
                .DistinctBy(p => p.Id)
                .Where(p => IsEligible(p, byProduct, byCategory, cartTotal));
        }

        private static (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) ComputePromoResult(
            PromotionGetDTO p,
            IEnumerable<CartItemGetDTO> items,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            return p.Reward switch
            {
                PromotionReward.PercentDiscount =>
                    (
                        (p.ProductId.HasValue
                            ? byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Total : 0
                            : p.CategoryId.HasValue
                                ? byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Total : 0
                                : cartTotal) * (p.RewardValue / 100m),
                        0,
                        (int?)null
                    ),

                PromotionReward.FreeItems when p.ProductId.HasValue =>
                    (
                        p.RewardValue * (byProduct.TryGetValue(p.ProductId.Value, out var prod)
                            ? prod.Total / prod.Quantity
                            : 0),
                        p.RewardValue,
                        p.ProductId
                    ),

                PromotionReward.FreeItems when p.CategoryId.HasValue =>
                    items
                        .Where(i => i.Product.Categories.Any(c => c.Id == p.CategoryId.Value))
                        .OrderBy(i => i.Product.Price)
                        .FirstOrDefault() is { } cheapest
                            ? (p.RewardValue * cheapest.Product.Price, p.RewardValue, (int?)cheapest.Product.Id)
                            : (0, 0, null),

                _ => (0, 0, null)
            };
        }

        internal static (decimal TotalSaved, int FreeItemQuantity, int? FreeItemProductId) GetBestPromotion(
            IEnumerable<CartItemGetDTO> items,
            decimal cartTotal)
        {
            var itemList = items.ToList();
            var (byProduct, byCategory) = ComputeCartAggregates(itemList);

            return GetEligiblePromotions(itemList, byProduct, byCategory, cartTotal)
                .Select(p => ComputePromoResult(p, itemList, byProduct, byCategory, cartTotal))
                .OrderByDescending(r => r.TotalSaved)
                .FirstOrDefault();
        }
    }
}