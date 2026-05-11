using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartShoppingAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoriesId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductsId",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductCategories",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "ProductCategories",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ProductsId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CategoryId");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gadgets, smartphones, audio, and home entertainment systems.", "Electronics" },
                    { 2, "Laptops, desktops, monitors, components, and networking gear.", "Computers & Accessories" },
                    { 3, "Furniture, decor, appliances, and kitchenware.", "Home & Kitchen" },
                    { 4, "Apparel and accessories for men, women, and kids.", "Clothing, Shoes & Jewelry" },
                    { 5, "Skincare, makeup, haircare, and grooming products.", "Beauty & Personal Care" },
                    { 6, "Fiction, non-fiction, educational, and audiobooks.", "Books" },
                    { 7, "Fitness equipment, outdoor gear, and athletic clothing.", "Sports & Outdoors" },
                    { 8, "Board games, action figures, puzzles, and educational toys.", "Toys & Games" },
                    { 9, "Vitamins, supplements, cleaning supplies, and medical care.", "Health & Household" },
                    { 10, "Car parts, accessories, tools, and car care products.", "Automotive" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "Tracks heart rate, sleep, and workouts with GPS.", "https://via.placeholder.com/300?text=Smartwatch", "Smart Fitness Watch", 199.99m, 45 },
                    { 2, "15.6-inch display, 16GB RAM, 1TB SSD, RTX 4060.", "https://via.placeholder.com/300?text=Gaming+Laptop", "Pro Gaming Laptop", 1299.00m, 12 },
                    { 3, "12-cup drip coffee maker with programmable timer.", "https://via.placeholder.com/300?text=Coffee+Maker", "Programmable Coffee Maker", 59.99m, 30 },
                    { 4, "Breathable, 100% cotton everyday crewneck t-shirt.", "https://via.placeholder.com/300?text=T-Shirt", "Men's Classic Cotton T-Shirt", 15.00m, 200 },
                    { 5, "Lightweight athletic sneakers with shock absorption.", "https://via.placeholder.com/300?text=Running+Shoes", "Women's Running Shoes", 75.00m, 80 },
                    { 6, "Gentle daily cleanser with hyaluronic acid.", "https://via.placeholder.com/300?text=Cleanser", "Hydrating Facial Cleanser", 18.50m, 90 },
                    { 7, "An epic space adventure across uncharted star systems.", "https://via.placeholder.com/300?text=Sci-Fi+Book", "The Great Galaxy - Sci-Fi Novel", 12.99m, 150 },
                    { 8, "Extra thick exercise mat with carrying strap.", "https://via.placeholder.com/300?text=Yoga+Mat", "Non-Slip Yoga Mat", 22.00m, 110 },
                    { 9, "Cooperative family board game for 2-6 players.", "https://via.placeholder.com/300?text=Board+Game", "Mystery Detective Board Game", 29.50m, 40 },
                    { 10, "Daily vitamins with essential nutrients for adults.", "https://via.placeholder.com/300?text=Vitamins", "Multivitamin Gummies", 14.99m, 130 },
                    { 11, "Front recording with night vision capabilities.", "https://via.placeholder.com/300?text=Dash+Cam", "1080p Dash Cam", 65.00m, 25 },
                    { 12, "Waterproof wireless speaker with 12-hour battery.", "https://via.placeholder.com/300?text=Speaker", "Portable Bluetooth Speaker", 45.99m, 65 },
                    { 13, "Comfortable grip with adjustable DPI.", "https://via.placeholder.com/300?text=Mouse", "Ergonomic Wireless Mouse", 29.99m, 120 },
                    { 14, "1000W motor for smoothies, soups, and crushing ice.", "https://via.placeholder.com/300?text=Blender", "High-Speed Blender", 89.99m, 20 },
                    { 15, "Insulated, water-resistant jacket for cold weather.", "https://via.placeholder.com/300?text=Winter+Jacket", "Winter Puffer Jacket", 110.00m, 55 },
                    { 16, "Brightening serum for uneven skin tones.", "https://via.placeholder.com/300?text=Face+Serum", "Vitamin C Face Serum", 24.00m, 85 },
                    { 17, "100 quick recipes for the whole family.", "https://via.placeholder.com/300?text=Cookbook", "Healthy Weeknight Meals Cookbook", 24.99m, 70 },
                    { 18, "Space-saving weights adjustable from 5 to 52 lbs.", "https://via.placeholder.com/300?text=Dumbbells", "Adjustable Dumbbell Set", 199.00m, 10 },
                    { 19, "500-piece colorful brick set for creative play.", "https://via.placeholder.com/300?text=Blocks", "Classic Building Blocks Set", 34.99m, 85 },
                    { 20, "Complete exterior cleaning and detailing bundle.", "https://via.placeholder.com/300?text=Car+Wash+Kit", "Premium Car Wash & Wax Kit", 42.99m, 35 }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "CategoryId", "IsActive", "Name", "ProductId", "Reward", "RewardValue", "Threshold", "Type" },
                values: new object[,]
                {
                    { 1, null, true, "10% Off Entire Order Over 500 RON", null, 1, 10, 500.00m, 1 },
                    { 2, null, true, "5% Off Entire Order Over 200 RON", null, 1, 5, 200.00m, 1 },
                    { 3, null, true, "Buy Any 5 Items, Get 15% Off Total", null, 1, 15, 5.00m, 0 },
                    { 4, null, false, "INACTIVE: 20% Off Mega Carts (1000+ RON)", null, 1, 20, 1000.00m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "admin@smartshopping.com", "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG", 1, "admin_master" },
                    { 2, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "john@example.com", "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG", 0, "john_doe" },
                    { 3, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "jane@example.com", "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG", 0, "jane_smith" },
                    { 4, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "test@example.com", "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG", 0, "tester_account" },
                    { 5, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "deals@example.com", "$2a$11$DVPyXpSFG6C.ZRtuQQ7I6..dnE1/l99RJwMbXuBcdDCy6R0W/jJTG", 0, "deal_hunter" }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 7, 1 },
                    { 9, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 7, 5 },
                    { 5, 6 },
                    { 6, 7 },
                    { 7, 8 },
                    { 8, 9 },
                    { 9, 10 },
                    { 1, 11 },
                    { 10, 11 },
                    { 1, 12 },
                    { 2, 13 },
                    { 3, 14 },
                    { 4, 15 },
                    { 5, 16 },
                    { 3, 17 },
                    { 6, 17 },
                    { 7, 18 },
                    { 8, 19 },
                    { 10, 20 }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "CategoryId", "IsActive", "Name", "ProductId", "Reward", "RewardValue", "Threshold", "Type" },
                values: new object[,]
                {
                    { 5, 1, true, "15% Off Electronics Over 300 RON", null, 1, 15, 300.00m, 1 },
                    { 6, 4, true, "Buy 3 Clothing Items, Get 1 Free", null, 0, 1, 3.00m, 0 },
                    { 7, 6, true, "20% Off Books When You Buy 2 or More", null, 1, 20, 2.00m, 0 },
                    { 8, 9, true, "5% Off Health Essentials Over 100 RON", null, 1, 5, 100.00m, 1 },
                    { 9, 8, false, "INACTIVE: Buy 2 Toys, Get 1 Free", null, 0, 1, 2.00m, 0 },
                    { 10, null, true, "10% Off Smart Fitness Watch", 1, 1, 10, 1.00m, 0 },
                    { 11, null, true, "Buy 2 T-Shirts, Get 1 Free", 4, 0, 1, 2.00m, 0 },
                    { 12, null, true, "15% Off Pro Gaming Laptop", 2, 1, 15, 1.00m, 0 },
                    { 13, null, true, "Buy 4 Multivitamin Gummies, Get 2 Free", 10, 0, 2, 4.00m, 0 },
                    { 14, null, false, "INACTIVE: 50% Off Mystery Board Game", 9, 1, 50, 1.00m, 0 }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartId", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 2, 1 },
                    { 2, 1, 13, 1 },
                    { 3, 1, 11, 1 },
                    { 4, 2, 7, 1 },
                    { 5, 2, 3, 1 },
                    { 6, 3, 6, 1 },
                    { 7, 3, 10, 4 },
                    { 8, 4, 4, 4 },
                    { 9, 4, 1, 1 },
                    { 10, 5, 9, 1 },
                    { 11, 5, 18, 2 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories");

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 7, 8 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 8, 9 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 9, 10 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 10, 11 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 12 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 14 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 15 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 5, 16 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 17 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 6, 17 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 7, 18 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 8, 19 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 10, 20 });

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductCategories",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductCategories",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoriesId",
                table: "ProductCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductsId",
                table: "ProductCategories",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
