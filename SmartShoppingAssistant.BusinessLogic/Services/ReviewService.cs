using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ReviewService(IReviewRepository reviewRepository, IMapper mapper, IUserRepository userRepository) : IReviewService
    {
        public async Task<ProductReviewGetDTO> GetByIdAsync(int reviewId)
        {
            var review = await reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");
            }
            return mapper.Map<ProductReviewGetDTO>(review);
        }
        public async Task<IEnumerable<ProductReviewGetDTO>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await reviewRepository.GetReviewsByProductIdAsync(productId);
            return mapper.Map<IEnumerable<ProductReviewGetDTO>>(reviews);
            
        }

        public async Task<IEnumerable<UserReviewGetDTO>> GetReviewsByUserIdAsync(int userId)
        {
            var reviews = await reviewRepository.GetReviewsByUserIdAsync(userId);
            return mapper.Map<IEnumerable<UserReviewGetDTO>>(reviews);
        }

        public async Task<ProductReviewGetDTO> AddReviewAsync(ReviewPostDTO reviewPostDTO, int userId)
        {
            var reviewEntity = mapper.Map<Review>(reviewPostDTO);
            reviewEntity.UserId = userId;
            reviewEntity.PostedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            reviewEntity.Likes = 0; // Initialize likes to 0 for a new review
            await reviewRepository.AddAsync(reviewEntity);

            reviewEntity.User = await userRepository.GetByIdAsync(reviewEntity.UserId);
            return mapper.Map<ProductReviewGetDTO>(reviewEntity);
        }

        public async Task<ProductReviewGetDTO> UpdateReviewAsync(int reviewId, ReviewPutDTO reviewPutDTO)
        {
            var reviewEntity = await reviewRepository.GetByIdAsync(reviewId);

            mapper.Map(reviewPutDTO, reviewEntity);

            await reviewRepository.UpdateAsync(reviewEntity);

            reviewEntity.User = await userRepository.GetByIdAsync(reviewEntity.UserId);

            return mapper.Map<ProductReviewGetDTO>(reviewEntity);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            await reviewRepository.DeleteAsync(reviewId);
        }
    }
}
