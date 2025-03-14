using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class hotfix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "automationid",
                table: "InComingCalls");

            migrationBuilder.DropColumn(
                name: "fullName",
                table: "InComingCalls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "automationid",
                table: "InComingCalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fullName",
                table: "InComingCalls",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
