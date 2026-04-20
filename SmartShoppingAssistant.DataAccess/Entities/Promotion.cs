namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Promotion
    {
        public enum PromotionType
        {
            CategoryPromotion,
            ProductPromotion,
        }
        public enum RewardType
        {
            LoyaltyPoints,
            Promotion,
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PromotionType Type { get; set; }
        public decimal Threshold { get; set; }
        public RewardType Reward { get; set; }
        public int RewardValue { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }

        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
