using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppuppgift_BU2.Migrations
{
    /// <inheritdoc />
    public partial class Iettan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "PurchasedItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "PurchasedItems");
        }
    }
}
