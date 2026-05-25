using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class ReviewSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    UserId = 2, // john_doe
                    ProductId = 1, // Wireless Mouse
                    Rating = 5,
                    Text = "Excellent mouse! Very responsive and comfortable to use.",
                    PostedAt = new DateOnly(2024, 1, 10)
                },
                new Review
                {
                    Id = 2,
                    UserId = 3, // jane_smith
                    ProductId = 1, // Wireless Mouse
                    Rating = 4,
                    Text = "Good value for the price. Battery life could be better.",
                    PostedAt = new DateOnly(2024, 1, 12)
                },
                new Review
                {
                    Id = 3,
                    UserId = 4, // tester_account
                    ProductId = 2, // Bluetooth Headphones
                    Rating = 3,
                    Text = "Sound quality is decent but not great. Comfortable fit though.",
                    PostedAt = new DateOnly(2024, 1, 15)
                },
                new Review
                {
                    Id = 4,
                    UserId = 5, // deal_hunter
                    ProductId = 3, // Smartwatch Pro
                    Rating = 5,
                    Text = "Love this smartwatch! Great features and battery life.",
                    PostedAt = new DateOnly(2024, 1, 20)
                },
                new Review
                {
                    Id = 5,
                    UserId = 2, // john_doe
                    ProductId = 3, // Smartwatch Pro
                    Rating = 4,
                    Text = "Very good smartwatch but a bit pricey.",
                    PostedAt = new DateOnly(2024, 1, 22)
                }
            );
        }
    }
}
