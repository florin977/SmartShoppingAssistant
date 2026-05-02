using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    public class PromotionProfile : Profile
    {
        public PromotionProfile()
        {
            CreateMap<PromotionPostDTO, Promotion>();
            CreateMap<PromotionPutDTO, Promotion>();
            CreateMap<Promotion, PromotionGetDTO>();
        }
    }
}
