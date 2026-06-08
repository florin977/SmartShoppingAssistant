using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Parameters;
using SmartShoppingAssistant.DataAccess.Repository;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;
using System.Transactions;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository, IMapper mapper, IUserRepository userRepository) : IReviewService
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
        public async Task<PagedResult<ProductReviewGetDTO>> GetReviewsByProductIdAsync(int productId, PaginationQueryDTO paginationQuery)
        {
            var paginationParameters = mapper.Map<PaginationParameters>(paginationQuery);
            var pagedResult = await reviewRepository.GetReviewsByProductIdAsync(productId, paginationParameters);
            var productReviewGetDto = mapper.Map<IEnumerable<ProductReviewGetDTO>>(pagedResult.Items);
            return new PagedResult<ProductReviewGetDTO>
            {
                Items = productReviewGetDto,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };
        }

        public async Task<PagedResult<UserReviewGetDTO>> GetReviewsByUserIdAsync(int userId, PaginationQueryDTO paginationQuery)
        {
            var paginationParameters = mapper.Map<PaginationParameters>(paginationQuery);
            var pagedResult = await reviewRepository.GetReviewsByUserIdAsync(userId, paginationParameters);
            var userReviewGetDto = mapper.Map<IEnumerable<UserReviewGetDTO>>(pagedResult.Items);
            return new PagedResult<UserReviewGetDTO>
            {
                Items = userReviewGetDto,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };
        }
        // Might have concurrency issues if multiple reviews are added/updated/deleted for the same product at the same time.
        public async Task<ProductReviewGetDTO> AddReviewAsync(ReviewPostDTO reviewPostDTO, int userId)
        {
            var reviewEntity = mapper.Map<Review>(reviewPostDTO);
            reviewEntity.UserId = userId;
            reviewEntity.PostedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            reviewEntity.UpdatedAt = null;
            reviewEntity.Likes = 0;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await reviewRepository.AddAsync(reviewEntity);

                var product = await productRepository.GetByIdAsync(reviewEntity.ProductId);

                product.Rating = ((product.Rating * product.ReviewsCount) + reviewEntity.Rating) / (product.ReviewsCount + 1);
                product.ReviewsCount++;

                await productRepository.UpdateAsync(product);

                transaction.Complete();
            }

            reviewEntity.User = await userRepository.GetByIdAsync(reviewEntity.UserId);
            return mapper.Map<ProductReviewGetDTO>(reviewEntity);
        }

        public async Task<ProductReviewGetDTO> UpdateReviewAsync(int reviewId, ReviewPutDTO reviewPutDTO)
        {
            var reviewEntity = await reviewRepository.GetByIdAsync(reviewId);

            reviewEntity.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow); 
            var oldRating = reviewEntity.Rating;

            mapper.Map(reviewPutDTO, reviewEntity);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await reviewRepository.UpdateAsync(reviewEntity);

                if (oldRating != reviewEntity.Rating)
                {
                    var product = await productRepository.GetByIdAsync(reviewEntity.ProductId);

                    decimal currentTotalStars = (product.Rating * product.ReviewsCount);
                    product.Rating = (currentTotalStars - oldRating + reviewEntity.Rating) / product.ReviewsCount;

                    await productRepository.UpdateAsync(product);
                }

                transaction.Complete();
            }

            reviewEntity.User = await userRepository.GetByIdAsync(reviewEntity.UserId);
            return mapper.Map<ProductReviewGetDTO>(reviewEntity);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var reviewEntity = await reviewRepository.GetByIdAsync(reviewId);
            if (reviewEntity == null) return;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var product = await productRepository.GetByIdAsync(reviewEntity.ProductId);

                if (product.ReviewsCount <= 1)
                {
                    product.Rating = 0;
                    product.ReviewsCount = 0;
                }
                else
                {
                    product.Rating = ((product.Rating * product.ReviewsCount) - reviewEntity.Rating) / (product.ReviewsCount - 1);
                    product.ReviewsCount--;
                }

                await productRepository.UpdateAsync(product);
                await reviewRepository.DeleteAsync(reviewId);

                transaction.Complete();
            }
        }
        public async Task<ProductReviewGetDTO> GetByProductAndUserId(int productId, int userId)
        {
            var review = await reviewRepository.GetByProductAndUserId(productId, userId);
            if (review == null)
            {
                return null;
            }
            return mapper.Map<ProductReviewGetDTO>(review);
        }
    }
}