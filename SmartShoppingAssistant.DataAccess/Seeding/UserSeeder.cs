using System;
using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Seeding
{
    public static class UserSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Placeholder hash ("LetMeIn")
            var commonHash = "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG";

            // =========================================================
            // 1. SEED USERS
            // =========================================================
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin_master",
                    Email = "admin@smartshopping.com",
                    PasswordHash = commonHash,
                    Role = UserRole.Admin,
                    CreatedAt = seedDate
                },
                new User
                {
                    Id = 2,
                    Username = "john_doe",
                    Email = "john@example.com",
                    PasswordHash = commonHash,
                    Role = UserRole.Customer,
                    CreatedAt = seedDate
                },
                new User
                {
                    Id = 3,
                    Username = "jane_smith",
                    Email = "jane@example.com",
                    PasswordHash = commonHash,
                    Role = UserRole.Customer,
                    CreatedAt = seedDate
                },
                new User
                {
                    Id = 4,
                    Username = "tester_account",
                    Email = "test@example.com",
                    PasswordHash = commonHash,
                    Role = UserRole.Customer,
                    CreatedAt = seedDate
                },
                new User
                {
                    Id = 5,
                    Username = "deal_hunter",
                    Email = "deals@example.com",
                    PasswordHash = commonHash,
                    Role = UserRole.Customer,
                    CreatedAt = seedDate
                }
            );

            // =========================================================
            // 2. SEED EMPTY CARTS (1-to-1 mapping with Users)
            // =========================================================
            modelBuilder.Entity<Cart>().HasData(
                new Cart { Id = 1, UserId = 1 }, // Admin's cart
                new Cart { Id = 2, UserId = 2 }, // John's cart
                new Cart { Id = 3, UserId = 3 }, // Jane's cart
                new Cart { Id = 4, UserId = 4 }, // Tester's cart
                new Cart { Id = 5, UserId = 5 }  // Deal Hunter's cart
            );
        }
    }
}