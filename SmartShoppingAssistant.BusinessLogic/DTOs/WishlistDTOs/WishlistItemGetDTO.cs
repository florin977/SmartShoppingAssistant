namespace SmartShoppingAssistant.BusinessLogic.DTOs.WishlistDTOs
{
    public class WishlistItemGetDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime AddedAt { get; set; }

        public ProductSummaryGetDTO Product { get; set; } = null!;
    }
}
