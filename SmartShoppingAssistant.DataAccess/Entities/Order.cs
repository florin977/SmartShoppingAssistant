namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Order
    {
        public enum StatusType
        {
            Pending,
            Completed,
            Shipping,
            Shipped,
            Cancelled
        };

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public StatusType Status { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime ExpectedArrival { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal DiscountTotal { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
