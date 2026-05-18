using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class CartItemSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasData(
                // =========================================================
                // Cart 1: Admin Master (High-Value Cart)
                // Goal: Test the "10% Off Entire Order Over 500 RON" 
                // and Category-specific "15% Off Electronics Over 300"
                // =========================================================
                new CartItem { Id = 1, CartId = 1, ProductId = 2, Quantity = 1, IsFreeGift = false },  // Pro Gaming Laptop (1299.00)
                new CartItem { Id = 2, CartId = 1, ProductId = 13, Quantity = 1, IsFreeGift = false }, // Ergonomic Mouse (29.99)
                new CartItem { Id = 3, CartId = 1, ProductId = 11, Quantity = 1, IsFreeGift = false }, // Dash Cam (65.00)

                // =========================================================
                // Cart 2: John Doe (Standard Buyer)
                // Goal: General cart logic and a smaller threshold test
                // =========================================================
                new CartItem { Id = 4, CartId = 2, ProductId = 7, Quantity = 1, IsFreeGift = false },  // Sci-Fi Novel
                new CartItem { Id = 5, CartId = 2, ProductId = 3, Quantity = 1, IsFreeGift = false },  // Coffee Maker

                // =========================================================
                // Cart 3: Jane Smith (Health & Vitamins)
                // Goal: Test the Product Promotion "Buy 4 Multivitamin Gummies, Get 2 Free"
                // =========================================================
                new CartItem { Id = 6, CartId = 3, ProductId = 6, Quantity = 1, IsFreeGift = false },  // Facial Cleanser
                new CartItem { Id = 7, CartId = 3, ProductId = 10, Quantity = 4, IsFreeGift = false }, // Multivitamin Gummies (Qty: 4)

                // =========================================================
                // Cart 4: Tester Account (Clothing Quantity Tester)
                // Goal: Test the Product Promotion "Buy 2 T-Shirts, Get 1 Free" 
                // and Category Promotion "Buy 3 Clothing Items, Get 1 Free"
                // =========================================================
                new CartItem { Id = 8, CartId = 4, ProductId = 4, Quantity = 4, IsFreeGift = false },  // Men's T-Shirt (Qty: 4)
                new CartItem { Id = 9, CartId = 4, ProductId = 1, Quantity = 1, IsFreeGift = false },  // Smart Fitness Watch

                // =========================================================
                // Cart 5: Deal Hunter
                // Goal: Attempting to trigger an INACTIVE promotion (Board Game)
                // =========================================================
                new CartItem { Id = 10, CartId = 5, ProductId = 9, Quantity = 1, IsFreeGift = false }, // Mystery Board Game
                new CartItem { Id = 11, CartId = 5, ProductId = 18, Quantity = 2, IsFreeGift = false } // Dumbbell Set
            );
        }
    }
}