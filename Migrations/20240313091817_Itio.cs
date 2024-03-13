using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppuppgift_BU2.Migrations
{
    /// <inheritdoc />
    public partial class Itio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Reviews",
                newName: "ReviewUserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewUserName",
                table: "Reviews",
                newName: "UserName");
        }
    }
}
