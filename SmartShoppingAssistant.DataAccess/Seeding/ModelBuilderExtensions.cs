using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAllData(this ModelBuilder modelBuilder)
        {
            // 1. Independent Entities
            CategorySeeder.Seed(modelBuilder);
            UserSeeder.Seed(modelBuilder);

            // 2. Dependent Entities
            ProductSeeder.Seed(modelBuilder);
            PromotionSeeder.Seed(modelBuilder);

            // 3. Lowest level dependencies
            CartItemSeeder.Seed(modelBuilder);
        }
    }
}