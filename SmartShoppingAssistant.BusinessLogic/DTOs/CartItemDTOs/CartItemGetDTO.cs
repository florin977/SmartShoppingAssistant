using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.CartItemDTOs
{
    public class CartItemGetDTO
    {
        public int Id { get; set; }
        //public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        //public bool isFreeGift { get; set; } = false;
    }
}
