using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<Category, CategoryGetDTO>()
                .ForMember(dest => dest.Promotions, opt => opt.Ignore());
            CreateMap<CategoryPostDTO, Category>();
            CreateMap<CategoryPutDTO, Category>();
            CreateMap<CategoryGetDTO, CategorySlimGetDTO>();
        }
    }
}
