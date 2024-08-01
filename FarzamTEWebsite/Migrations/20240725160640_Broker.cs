using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Broker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Broker",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Broker",
                table: "HappyCalls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Broker",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Broker",
                table: "HappyCalls");
        }
    }
}
