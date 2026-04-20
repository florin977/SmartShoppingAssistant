using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(rt => rt.Id);
            builder.HasIndex(rt => rt.Token).IsUnique();

            builder.Property(rt => rt.UserId).IsRequired();
            builder.Property(rt => rt.Token).IsRequired().HasMaxLength(255);
            builder.Property(rt => rt.JwtId).IsRequired();
            builder.Property(rt => rt.ExpiresAt).IsRequired();
            builder.Property(rt => rt.RevokedAt).IsRequired(false);
        }
    }
}
