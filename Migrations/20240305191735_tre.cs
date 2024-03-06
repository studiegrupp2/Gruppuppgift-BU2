using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppuppgift_BU2.Migrations
{
    /// <inheritdoc />
    public partial class tre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Products",
                newName: "AverageRating");

            migrationBuilder.AddColumn<List<double>>(
                name: "ratingList",
                table: "Products",
                type: "double precision[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ratingList",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "AverageRating",
                table: "Products",
                newName: "Rating");
        }
    }
}
