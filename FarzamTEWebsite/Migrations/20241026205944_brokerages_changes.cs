using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class brokerages_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Brokerages");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Brokerages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Brokerages");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Brokerages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
