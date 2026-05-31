using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductSummaryGetDTO>();
            CreateMap<ProductPostDTO, Product>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<ProductPutDTO, Product>();
            CreateMap<Product, ProductGetDTO>()
                .ForMember(dest => dest.Promotions, opt => opt.Ignore());
        }
    }
}
