using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.ReviewDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.UserDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<User, UserSummaryGetDTO>();
            CreateMap<Product, ProductSummaryGetDTO>();

            CreateMap<Review, ProductReviewGetDTO>();
            CreateMap<Review, UserReviewGetDTO>();
            CreateMap<ReviewPostDTO, Review>();
            CreateMap<ReviewPutDTO, Review>();
        }
    }
}