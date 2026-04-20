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
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
