namespace SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs
{
    public class UserReviewGetDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateTime PostedAt { get; set; }
        public int Likes { get; set; }

        // The flattened product data
        public ProductSummaryGetDTO Product { get; set; } = null!;
    }
}