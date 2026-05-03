using AutoMapper;
using Microsoft.Identity.Client;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile() 
        {
            CreateMap<Cart, CartGetDTO>();
            CreateMap<CartCreateDTO, Cart>();
        }
    }
}
