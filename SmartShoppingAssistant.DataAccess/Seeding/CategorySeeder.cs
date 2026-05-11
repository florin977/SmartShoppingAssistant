using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class CategorySeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    Description = "Gadgets, smartphones, audio, and home entertainment systems."
                },
                new Category
                {
                    Id = 2,
                    Name = "Computers & Accessories",
                    Description = "Laptops, desktops, monitors, components, and networking gear."
                },
                new Category
                {
                    Id = 3,
                    Name = "Home & Kitchen",
                    Description = "Furniture, decor, appliances, and kitchenware."
                },
                new Category
                {
                    Id = 4,
                    Name = "Clothing, Shoes & Jewelry",
                    Description = "Apparel and accessories for men, women, and kids."
                },
                new Category
                {
                    Id = 5,
                    Name = "Beauty & Personal Care",
                    Description = "Skincare, makeup, haircare, and grooming products."
                },
                new Category
                {
                    Id = 6,
                    Name = "Books",
                    Description = "Fiction, non-fiction, educational, and audiobooks."
                },
                new Category
                {
                    Id = 7,
                    Name = "Sports & Outdoors",
                    Description = "Fitness equipment, outdoor gear, and athletic clothing."
                },
                new Category
                {
                    Id = 8,
                    Name = "Toys & Games",
                    Description = "Board games, action figures, puzzles, and educational toys."
                },
                new Category
                {
                    Id = 9,
                    Name = "Health & Household",
                    Description = "Vitamins, supplements, cleaning supplies, and medical care."
                },
                new Category
                {
                    Id = 10,
                    Name = "Automotive",
                    Description = "Car parts, accessories, tools, and car care products."
                }
            );
        }
    }
}