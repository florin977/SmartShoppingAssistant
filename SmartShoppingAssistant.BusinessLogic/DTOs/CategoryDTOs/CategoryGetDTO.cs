using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs
{
    public class CategoryGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<PromotionGetDTO> Promotions { get; set; } = new List<PromotionGetDTO>();
    }
}
