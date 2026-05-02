using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile() 
        {
            CreateMap<CartItem, CartItemGetDTO>();
            CreateMap<CartItemPostDTO, CartItem>();
            CreateMap<CartItemPutDTO, CartItem>();
        }
    }
}
