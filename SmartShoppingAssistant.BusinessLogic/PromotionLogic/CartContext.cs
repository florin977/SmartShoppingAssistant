using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.PromotionLogic
{
    public class CartContext
    {
        public ICollection<CartItem> Items { get; }
        public List<CartAppliedPromotionDTO> AppliedPromotions { get; }
        public decimal RunningTotal { get; set; }
        public decimal TotalSavedOverall { get; set; }

        public Dictionary<int, (int Quantity, decimal Total)> ByProduct { get; }
        public Dictionary<int, (int Quantity, decimal Total)> ByCategory { get; }

        public CartContext(
            ICollection<CartItem> items,
            List<CartAppliedPromotionDTO> appliedPromotions,
            decimal runningTotal,
            decimal totalSavedOverall)
        {
            Items = items;
            AppliedPromotions = appliedPromotions;
            RunningTotal = runningTotal;
            TotalSavedOverall = totalSavedOverall;

            // Only count items that are not already free gifts for aggregates
            var paidItems = items.Where(i => !i.IsFreeGift).ToList();

            ByProduct = paidItems
                .GroupBy(i => i.Product.Id)
                .ToDictionary(g => g.Key, g => (
                    Quantity: g.Sum(i => i.Quantity),
                    Total: g.Sum(i => i.Quantity * i.Product.Price)));

            ByCategory = paidItems
                .SelectMany(i => i.Product.Categories, (i, c) => (Category: c, Item: i))
                .GroupBy(x => x.Category.Id)
                .ToDictionary(g => g.Key, g => (
                    Quantity: g.Sum(x => x.Item.Quantity),
                    Total: g.Sum(x => x.Item.Quantity * x.Item.Product.Price)));
        }

        public decimal GetApplicableTotal(Promotion p)
        {
            if (p.ProductId.HasValue) return ByProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Total : 0;
            if (p.CategoryId.HasValue) return ByCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Total : 0;
            return RunningTotal;
        }

        public int GetApplicableQuantity(Promotion p)
        {
            if (p.ProductId.HasValue) return ByProduct.TryGetValue(p.ProductId.Value, out var prod) ? prod.Quantity : 0;
            if (p.CategoryId.HasValue) return ByCategory.TryGetValue(p.CategoryId.Value, out var cat) ? cat.Quantity : 0;
            return ByProduct.Values.Sum(p => p.Quantity);
        }

        public CartItem? GetCheapestApplicableItem(Promotion p)
        {
            var validItems = Items.Where(i => !i.IsFreeGift);
            if (p.ProductId.HasValue) return validItems.Where(i => i.Product.Id == p.ProductId.Value).MinBy(i => i.Product.Price);
            if (p.CategoryId.HasValue) return validItems.Where(i => i.Product.Categories.Any(c => c.Id == p.CategoryId.Value)).MinBy(i => i.Product.Price);
            return validItems.MinBy(i => i.Product.Price);
        }
    }
}