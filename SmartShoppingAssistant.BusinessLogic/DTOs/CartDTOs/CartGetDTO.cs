using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs
{
    public class CartGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<CartItemGetDTO> CartItems { get; set; } = new List<CartItemGetDTO>();
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
    }
}
