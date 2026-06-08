namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string? Text { get; set; }
        public DateOnly PostedAt { get; set; }
        public DateOnly? UpdatedAt { get; set; }
        public int Likes { get; set; }

        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
