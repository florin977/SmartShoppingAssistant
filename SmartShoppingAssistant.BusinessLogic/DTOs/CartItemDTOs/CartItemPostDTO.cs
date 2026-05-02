using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs
{
    public class CartItemPostDTO
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
