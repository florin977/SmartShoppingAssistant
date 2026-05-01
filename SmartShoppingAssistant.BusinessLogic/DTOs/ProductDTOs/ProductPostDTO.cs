namespace SmartShoppingAssistant.BusinessLogic.DTOs
{
    public class ProductPostDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}