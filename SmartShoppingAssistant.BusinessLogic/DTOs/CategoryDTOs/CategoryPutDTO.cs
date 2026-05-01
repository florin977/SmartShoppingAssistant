using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs
{
    public class CategoryPutDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
