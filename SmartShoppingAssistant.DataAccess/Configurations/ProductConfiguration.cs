using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    { 
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(p => p.ImageUrl).HasMaxLength(500);
            builder.Property(p => p.Stock).IsRequired();
            // Ensure the stock never goes negative
            builder.ToTable(t => t.HasCheckConstraint("CK_Product_Stock_NonNegative", "[Stock] >= 0"));

            builder.HasMany(p => p.Categories)
                   .WithMany(c => c.Products)
                   .UsingEntity(j => j.ToTable("ProductCategories"));

            builder.HasMany(p => p.Promotions)
                   .WithOne(pr => pr.Product)
                   .HasForeignKey(pr => pr.ProductId)
                   .IsRequired(false);

            // On main branch this is a one-to-one relationship
            builder.HasMany(p => p.CartItems)
                   .WithOne(ci => ci.Product)
                   .HasForeignKey(ci => ci.ProductId);
        }
    }
}
