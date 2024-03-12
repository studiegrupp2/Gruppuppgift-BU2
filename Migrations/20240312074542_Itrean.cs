using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppuppgift_BU2.Migrations
{
    /// <inheritdoc />
    public partial class Itrean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ProductPrice",
                table: "PurchasedItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "PurchasedItems");
        }
    }
}
