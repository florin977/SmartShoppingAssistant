using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public enum PromotionType
        {
            CategoryPromotion,
            ProductPromotion,
        }
        public PromotionType Type { get; set; }
        public decimal Threshold { get; set; }
        public enum RewardType
        {
            LoyaltyPoints,
            Promotion,
        }
        public RewardType Reward { get; set; }
        public int RewardValue { get; set; }
        public bool IsActive { get; set; }

        public int ProductId { get; set; }
        // Auto-included navigation property for the related Product entity
        public Product Product { get; set; } = null!;
        public int CategoryId { get; set; }
        // Auto-included navigation property for the related Category entity
        public Category Category { get; set; } = null!;

    }
}
