using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShoppingAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Wishlist2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Users_UserId1",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_UserId1",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "WishlistItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "WishlistItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_UserId1",
                table: "WishlistItems",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Users_UserId1",
                table: "WishlistItems",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
