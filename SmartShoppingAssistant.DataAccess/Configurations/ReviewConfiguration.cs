using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ProductId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Text).HasMaxLength(1000);
            builder.Property(r => r.PostedAt).IsRequired();
        }
    }
}
