using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Migrations
{
    /// <inheritdoc />
    public partial class removedOneRowFromEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Weathers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Weathers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
