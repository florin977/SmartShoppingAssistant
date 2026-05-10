using SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CartDTOs
{
    public class CartGetDTO
    {
        public List<CartItemGetDTO> Items { get; set; } = new List<CartItemGetDTO>();
        public decimal Subtotal { get; set; }
        public List<CartAppliedPromotionDTO> AppliedPromotions { get; set; } = new List<CartAppliedPromotionDTO>();
        public decimal TotalDiscount { get; set; }
        public decimal Total {  get; set; }

        /*
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<CartItemGetDTO> CartItems { get; set; } = new List<CartItemGetDTO>();
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public List<PromotionGetDTO> ActivePromotions { get; set; } = new List<PromotionGetDTO>();
        public PromotionGetDTO? AppliedPromotion { get; set; }
        */
    }
}
