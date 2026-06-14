using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.WishlistDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<WishlistItem, WishlistItemGetDTO>();
            CreateMap<WishlistItemPostDTO, WishlistItem>();
        }
    }
}
