using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class transaction_st_m_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BOBT_Sandogh_Algorithm",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BOBT_Sandogh_Online",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FI_Moshtaghe_Group",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FI_Moshtaghe_Normal",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FI_Moshtaghe_Other",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FI_Moshtaghe_Station",
                table: "Transaction_Statistics_M",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BOBT_Sandogh_Algorithm",
                table: "Transaction_Statistics_M");

            migrationBuilder.DropColumn(
                name: "BOBT_Sandogh_Online",
                table: "Transaction_Statistics_M");

            migrationBuilder.DropColumn(
                name: "FI_Moshtaghe_Group",
                table: "Transaction_Statistics_M");

            migrationBuilder.DropColumn(
                name: "FI_Moshtaghe_Normal",
                table: "Transaction_Statistics_M");

            migrationBuilder.DropColumn(
                name: "FI_Moshtaghe_Other",
                table: "Transaction_Statistics_M");

            migrationBuilder.DropColumn(
                name: "FI_Moshtaghe_Station",
                table: "Transaction_Statistics_M");
        }
    }
}
