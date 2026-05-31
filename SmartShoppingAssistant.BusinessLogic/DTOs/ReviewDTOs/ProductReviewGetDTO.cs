using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs
{
    public class ProductReviewGetDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateOnly PostedAt { get; set; }
        public int Likes { get; set; }

        public UserSummaryGetDTO User { get; set; } = null!;
    }
}