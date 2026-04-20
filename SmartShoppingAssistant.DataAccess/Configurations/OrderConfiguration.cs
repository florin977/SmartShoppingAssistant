using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.Property(o => o.PlacedAt).IsRequired();
            builder.Property(o => o.ExpectedArrival).IsRequired();
            builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(o => o.ShippingCost).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(o => o.DiscountTotal).IsRequired().HasColumnType("decimal(10,2)");

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId);
        }
    }
}
