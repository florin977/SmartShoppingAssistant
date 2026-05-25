using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartShoppingAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReviewSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Likes", "PostedAt", "ProductId", "Rating", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, 0, new DateOnly(2024, 1, 10), 1, 5, "Excellent mouse! Very responsive and comfortable to use.", 2 },
                    { 2, 0, new DateOnly(2024, 1, 12), 1, 4, "Good value for the price. Battery life could be better.", 3 },
                    { 3, 0, new DateOnly(2024, 1, 15), 2, 3, "Sound quality is decent but not great. Comfortable fit though.", 4 },
                    { 4, 0, new DateOnly(2024, 1, 20), 3, 5, "Love this smartwatch! Great features and battery life.", 5 },
                    { 5, 0, new DateOnly(2024, 1, 22), 3, 4, "Very good smartwatch but a bit pricey.", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
