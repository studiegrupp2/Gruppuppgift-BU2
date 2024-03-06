using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppuppgift_BU2.Migrations
{
    /// <inheritdoc />
    public partial class Fyra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
