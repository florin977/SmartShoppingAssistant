using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs
{
    public class ReviewPutDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Review text cannot exceed 1000 characters.")]
        public string? Text { get; set; }
    }
}