using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class ProductSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // 1. Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smart Fitness Watch", Description = "Tracks heart rate, sleep, and workouts with GPS.", Price = 199.99m, ImageUrl = "https://via.placeholder.com/300?text=Smartwatch", Stock = 45 },
                new Product { Id = 2, Name = "Pro Gaming Laptop", Description = "15.6-inch display, 16GB RAM, 1TB SSD, RTX 4060.", Price = 1299.00m, ImageUrl = "https://via.placeholder.com/300?text=Gaming+Laptop", Stock = 12 },
                new Product { Id = 3, Name = "Programmable Coffee Maker", Description = "12-cup drip coffee maker with programmable timer.", Price = 59.99m, ImageUrl = "https://via.placeholder.com/300?text=Coffee+Maker", Stock = 30 },
                new Product { Id = 4, Name = "Men's Classic Cotton T-Shirt", Description = "Breathable, 100% cotton everyday crewneck t-shirt.", Price = 15.00m, ImageUrl = "https://via.placeholder.com/300?text=T-Shirt", Stock = 200 },
                new Product { Id = 5, Name = "Women's Running Shoes", Description = "Lightweight athletic sneakers with shock absorption.", Price = 75.00m, ImageUrl = "https://via.placeholder.com/300?text=Running+Shoes", Stock = 80 },
                new Product { Id = 6, Name = "Hydrating Facial Cleanser", Description = "Gentle daily cleanser with hyaluronic acid.", Price = 18.50m, ImageUrl = "https://via.placeholder.com/300?text=Cleanser", Stock = 90 },
                new Product { Id = 7, Name = "The Great Galaxy - Sci-Fi Novel", Description = "An epic space adventure across uncharted star systems.", Price = 12.99m, ImageUrl = "https://via.placeholder.com/300?text=Sci-Fi+Book", Stock = 150 },
                new Product { Id = 8, Name = "Non-Slip Yoga Mat", Description = "Extra thick exercise mat with carrying strap.", Price = 22.00m, ImageUrl = "https://via.placeholder.com/300?text=Yoga+Mat", Stock = 110 },
                new Product { Id = 9, Name = "Mystery Detective Board Game", Description = "Cooperative family board game for 2-6 players.", Price = 29.50m, ImageUrl = "https://via.placeholder.com/300?text=Board+Game", Stock = 40 },
                new Product { Id = 10, Name = "Multivitamin Gummies", Description = "Daily vitamins with essential nutrients for adults.", Price = 14.99m, ImageUrl = "https://via.placeholder.com/300?text=Vitamins", Stock = 130 },
                new Product { Id = 11, Name = "1080p Dash Cam", Description = "Front recording with night vision capabilities.", Price = 65.00m, ImageUrl = "https://via.placeholder.com/300?text=Dash+Cam", Stock = 25 },
                new Product { Id = 12, Name = "Portable Bluetooth Speaker", Description = "Waterproof wireless speaker with 12-hour battery.", Price = 45.99m, ImageUrl = "https://via.placeholder.com/300?text=Speaker", Stock = 65 },
                new Product { Id = 13, Name = "Ergonomic Wireless Mouse", Description = "Comfortable grip with adjustable DPI.", Price = 29.99m, ImageUrl = "https://via.placeholder.com/300?text=Mouse", Stock = 120 },
                new Product { Id = 14, Name = "High-Speed Blender", Description = "1000W motor for smoothies, soups, and crushing ice.", Price = 89.99m, ImageUrl = "https://via.placeholder.com/300?text=Blender", Stock = 20 },
                new Product { Id = 15, Name = "Winter Puffer Jacket", Description = "Insulated, water-resistant jacket for cold weather.", Price = 110.00m, ImageUrl = "https://via.placeholder.com/300?text=Winter+Jacket", Stock = 55 },
                new Product { Id = 16, Name = "Vitamin C Face Serum", Description = "Brightening serum for uneven skin tones.", Price = 24.00m, ImageUrl = "https://via.placeholder.com/300?text=Face+Serum", Stock = 85 },
                new Product { Id = 17, Name = "Healthy Weeknight Meals Cookbook", Description = "100 quick recipes for the whole family.", Price = 24.99m, ImageUrl = "https://via.placeholder.com/300?text=Cookbook", Stock = 70 },
                new Product { Id = 18, Name = "Adjustable Dumbbell Set", Description = "Space-saving weights adjustable from 5 to 52 lbs.", Price = 199.00m, ImageUrl = "https://via.placeholder.com/300?text=Dumbbells", Stock = 10 },
                new Product { Id = 19, Name = "Classic Building Blocks Set", Description = "500-piece colorful brick set for creative play.", Price = 34.99m, ImageUrl = "https://via.placeholder.com/300?text=Blocks", Stock = 85 },
                new Product { Id = 20, Name = "Premium Car Wash & Wax Kit", Description = "Complete exterior cleaning and detailing bundle.", Price = 42.99m, ImageUrl = "https://via.placeholder.com/300?text=Car+Wash+Kit", Stock = 35 }
            );

            // 2. Seed Join Table (ProductCategories)
            // Maps Products to Categories, demonstrating single and multiple category assignments.
            modelBuilder.Entity("ProductCategories").HasData(
                // Overlapping Products
                new { ProductId = 1, CategoryId = 1 }, // Smartwatch -> Electronics
                new { ProductId = 1, CategoryId = 7 }, // Smartwatch -> Sports
                new { ProductId = 1, CategoryId = 9 }, // Smartwatch -> Health

                new { ProductId = 2, CategoryId = 1 }, // Gaming Laptop -> Electronics
                new { ProductId = 2, CategoryId = 2 }, // Gaming Laptop -> Computers

                new { ProductId = 5, CategoryId = 4 }, // Running Shoes -> Clothing
                new { ProductId = 5, CategoryId = 7 }, // Running Shoes -> Sports

                new { ProductId = 11, CategoryId = 1 }, // Dash Cam -> Electronics
                new { ProductId = 11, CategoryId = 10 },// Dash Cam -> Auto

                new { ProductId = 17, CategoryId = 3 }, // Cookbook -> Home
                new { ProductId = 17, CategoryId = 6 }, // Cookbook -> Books

                // Standard Single Category Products
                new { ProductId = 3, CategoryId = 3 },  // Coffee Maker
                new { ProductId = 4, CategoryId = 4 },  // T-Shirt
                new { ProductId = 6, CategoryId = 5 },  // Cleanser
                new { ProductId = 7, CategoryId = 6 },  // Sci-Fi Novel
                new { ProductId = 8, CategoryId = 7 },  // Yoga Mat
                new { ProductId = 9, CategoryId = 8 },  // Board Game
                new { ProductId = 10, CategoryId = 9 }, // Vitamins
                new { ProductId = 12, CategoryId = 1 }, // Speaker
                new { ProductId = 13, CategoryId = 2 }, // Mouse
                new { ProductId = 14, CategoryId = 3 }, // Blender
                new { ProductId = 15, CategoryId = 4 }, // Jacket
                new { ProductId = 16, CategoryId = 5 }, // Face Serum
                new { ProductId = 18, CategoryId = 7 }, // Dumbbells
                new { ProductId = 19, CategoryId = 8 }, // Blocks
                new { ProductId = 20, CategoryId = 10 } // Car Wash Kit
            );
        }
    }
}