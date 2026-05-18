using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile() 
        {
            CreateMap<CartItem, CartItemGetDTO>()
                // Tell AutoMapper to force the price to 0 if it's a free gift
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                    src.IsFreeGift ? 0 : src.Product.Price))
                // Tell AutoMapper to force the subtotal to 0 if it's a free gift
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src =>
                    src.IsFreeGift ? 0 : (src.Quantity * src.Product.Price)));

            CreateMap<CartItemPostDTO, CartItem>();
            CreateMap<CartItemPutDTO, CartItem>();
        }
    }
}
