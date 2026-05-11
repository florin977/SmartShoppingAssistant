using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
// Make sure to include the namespace where your enums are located!
// using SmartShoppingAssistant.DataAccess.Entities.Enums; 

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class PromotionSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promotion>().HasData(
                // =========================================================
                // 1. CART-WIDE PROMOTIONS (Both ProductId & CategoryId are NULL)
                // =========================================================
                new Promotion 
                { 
                    Id = 1, Name = "10% Off Entire Order Over 500 RON", 
                    Type = PromotionType.CartTotal, Threshold = 500.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 10, 
                    ProductId = null, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 2, Name = "5% Off Entire Order Over 200 RON", 
                    Type = PromotionType.CartTotal, Threshold = 200.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 5, 
                    ProductId = null, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 3, Name = "Buy Any 5 Items, Get 15% Off Total", 
                    Type = PromotionType.Quantity, Threshold = 5.00m, // 5 items in cart
                    Reward = PromotionReward.PercentDiscount, RewardValue = 15, 
                    ProductId = null, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 4, Name = "INACTIVE: 20% Off Mega Carts (1000+ RON)", 
                    Type = PromotionType.CartTotal, Threshold = 1000.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 20, 
                    ProductId = null, CategoryId = null, IsActive = false // <-- Inactive test
                },

                // =========================================================
                // 2. CATEGORY-SPECIFIC (CategoryId has value, ProductId is NULL)
                // =========================================================
                new Promotion 
                { 
                    Id = 5, Name = "15% Off Electronics Over 300 RON", 
                    Type = PromotionType.CartTotal, Threshold = 300.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 15, 
                    ProductId = null, CategoryId = 1, IsActive = true // 1 = Electronics
                },
                new Promotion 
                { 
                    Id = 6, Name = "Buy 3 Clothing Items, Get 1 Free", 
                    Type = PromotionType.Quantity, Threshold = 3.00m, // Buy 3
                    Reward = PromotionReward.FreeItems, RewardValue = 1, // Get 1 free
                    ProductId = null, CategoryId = 4, IsActive = true // 4 = Clothing
                },
                new Promotion 
                { 
                    Id = 7, Name = "20% Off Books When You Buy 2 or More", 
                    Type = PromotionType.Quantity, Threshold = 2.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 20, 
                    ProductId = null, CategoryId = 6, IsActive = true // 6 = Books
                },
                new Promotion 
                { 
                    Id = 8, Name = "5% Off Health Essentials Over 100 RON", 
                    Type = PromotionType.CartTotal, Threshold = 100.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 5, 
                    ProductId = null, CategoryId = 9, IsActive = true // 9 = Health
                },
                new Promotion 
                { 
                    Id = 9, Name = "INACTIVE: Buy 2 Toys, Get 1 Free", 
                    Type = PromotionType.Quantity, Threshold = 2.00m, 
                    Reward = PromotionReward.FreeItems, RewardValue = 1, 
                    ProductId = null, CategoryId = 8, IsActive = false // <-- Inactive test
                },

                // =========================================================
                // 3. PRODUCT-SPECIFIC (ProductId has value, CategoryId is NULL)
                // =========================================================
                new Promotion 
                { 
                    // No threshold needed if you just buy 1, so threshold is 1
                    Id = 10, Name = "10% Off Smart Fitness Watch", 
                    Type = PromotionType.Quantity, Threshold = 1.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 10, 
                    ProductId = 1, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 11, Name = "Buy 2 T-Shirts, Get 1 Free", 
                    Type = PromotionType.Quantity, Threshold = 2.00m, 
                    Reward = PromotionReward.FreeItems, RewardValue = 1, 
                    ProductId = 4, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 12, Name = "15% Off Pro Gaming Laptop", 
                    Type = PromotionType.Quantity, Threshold = 1.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 15, 
                    ProductId = 2, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 13, Name = "Buy 4 Multivitamin Gummies, Get 2 Free", 
                    Type = PromotionType.Quantity, Threshold = 4.00m, 
                    Reward = PromotionReward.FreeItems, RewardValue = 2, 
                    ProductId = 10, CategoryId = null, IsActive = true 
                },
                new Promotion 
                { 
                    Id = 14, Name = "INACTIVE: 50% Off Mystery Board Game", 
                    Type = PromotionType.Quantity, Threshold = 1.00m, 
                    Reward = PromotionReward.PercentDiscount, RewardValue = 50, 
                    ProductId = 9, CategoryId = null, IsActive = false // <-- Inactive test
                }
            );
        }
    }
}