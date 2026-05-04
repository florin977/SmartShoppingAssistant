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

        // Returns the total price of the promotion's scope:
        // product total, category total, or the entire cart total if no scope is specified.
        private static decimal GetApplicableTotal(
            PromotionGetDTO p,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory,
            decimal cartTotal)
        {
            if (p.ProductId.HasValue)
                return byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Total : 0;

            if (p.CategoryId.HasValue)
                return byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Total : 0;

            return cartTotal;
        }

        // Returns the total quantity of the promotion's scope:
        // product quantity, category quantity, or the entire cart quantity if no scope is specified.
        private static int GetApplicableQuantity(
            PromotionGetDTO p,
            Dictionary<int, (int Quantity, decimal Total)> byProduct,
            Dictionary<int, (int Quantity, decimal Total)> byCategory)
        {
            if (p.ProductId.HasValue)
                return byProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Quantity : 0;

            if (p.CategoryId.HasValue)
                return byCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Quantity : 0;

            return byProduct.Values.Sum(p => p.Quantity);
        }

        // Returns the cheapest item in the promotion's scope:
        // within the product, category, or the entire cart if no scope is specified.
        private static CartItemGetDTO? GetCheapestItem(
            PromotionGetDTO p,
            IEnumerable<CartItemGetDTO> items)
        {
            if (p.ProductId.HasValue)
                return items
                    .Where(i => i.Product.Id == p.ProductId.Value)
                    .OrderBy(i => i.Product.Price)
                    .FirstOrDefault();

            if (p.CategoryId.HasValue)
                return items
                    .Where(i => i.Product.Categories.Any(c => c.Id == p.CategoryId.Value))
                    .OrderBy(i => i.Product.Price)
                    .FirstOrDefault();

            return items
                .OrderBy(i => i.Product.Price)
                .FirstOrDefault();
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
                PromotionType.Quantity => GetApplicableQuantity(p, byProduct, byCategory) >= p.Threshold,
                PromotionType.CartTotal => GetApplicableTotal(p, byProduct, byCategory, cartTotal) >= p.Threshold,
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
                    (GetApplicableTotal(p, byProduct, byCategory, cartTotal) * (p.RewardValue / 100m), 0, null),

                PromotionReward.FreeItems =>
                    GetCheapestItem(p, items) is { } cheapest
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