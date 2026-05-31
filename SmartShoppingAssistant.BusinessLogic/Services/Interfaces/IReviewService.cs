using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IReviewService
    {
        // Read Operations
        Task<IEnumerable<ProductReviewGetDTO>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<UserReviewGetDTO>> GetReviewsByUserIdAsync(int userId);
        Task<ProductReviewGetDTO> GetByIdAsync(int reviewId);

        // Write Operations
        Task<ProductReviewGetDTO> AddReviewAsync(ReviewPostDTO reviewPostDTO, int userId);
        Task<ProductReviewGetDTO> UpdateReviewAsync(int reviewId, ReviewPutDTO reviewPutDTO);
        Task DeleteReviewAsync(int reviewId);
    }
}