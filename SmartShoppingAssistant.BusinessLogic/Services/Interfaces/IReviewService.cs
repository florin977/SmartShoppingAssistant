using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;
using SmartShoppingAssistant.DataAccess.Parameters;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IReviewService
    {
        // Read Operations
        Task<PagedResult<ProductReviewGetDTO>> GetReviewsByProductIdAsync(int productId, PaginationQueryDTO paginationParameters);
        Task<PagedResult<UserReviewGetDTO>> GetReviewsByUserIdAsync(int userId, PaginationQueryDTO paginationParameters);
        Task<ProductReviewGetDTO> GetByIdAsync(int reviewId);

        // Write Operations
        Task<ProductReviewGetDTO> AddReviewAsync(ReviewPostDTO reviewPostDTO, int userId);
        Task<ProductReviewGetDTO> UpdateReviewAsync(int reviewId, ReviewPutDTO reviewPutDTO);
        Task DeleteReviewAsync(int reviewId); 
        Task<ProductReviewGetDTO> GetByProductAndUserId(int productId, int userId);


    }
}