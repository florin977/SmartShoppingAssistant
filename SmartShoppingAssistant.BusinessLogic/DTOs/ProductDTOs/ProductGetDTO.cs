using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.DTOs
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        // Automapper handles category -> categoryDTO mapping, so I can directly use CategoryGetDTO here
        public ICollection<CategoryGetDTO> Categories { get; set; } = null!;
    }
}