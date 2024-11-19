using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class amarmoamelat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brokerages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokerages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction_Statistics_M",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brokerage_ID = table.Column<int>(type: "int", nullable: false),
                    BOBT_Oragh_Bedehi_Online = table.Column<int>(type: "int", nullable: true),
                    BOBT_Oragh_Bedehi_Normal = table.Column<int>(type: "int", nullable: true),
                    BOBT_Moshtaghe_Online = table.Column<int>(type: "int", nullable: true),
                    BOBT_Moshtaghe_Normal = table.Column<int>(type: "int", nullable: true),
                    BOBT_Sarmaye_Herfei_Online = table.Column<int>(type: "int", nullable: true),
                    BOBT_Sarmaye_Herfei_Normal = table.Column<int>(type: "int", nullable: true),
                    BOBT_Sarmaye_Herfei_Algorithm = table.Column<int>(type: "int", nullable: true),
                    BOBT_saham_Online = table.Column<int>(type: "int", nullable: true),
                    BOBT_saham_Normal = table.Column<int>(type: "int", nullable: true),
                    BOBT_saham_Algorithm = table.Column<int>(type: "int", nullable: true),
                    BOBT_Total_Value = table.Column<int>(type: "int", nullable: true),
                    FI_Brokerage_Station = table.Column<int>(type: "int", nullable: true),
                    FI_Online_Normal = table.Column<int>(type: "int", nullable: true),
                    FI_Online_Group = table.Column<int>(type: "int", nullable: true),
                    FI_Online_Other = table.Column<int>(type: "int", nullable: true),
                    FI_Total_Value = table.Column<int>(type: "int", nullable: true),
                    BOBT_AND_FI_Total_Value = table.Column<int>(type: "int", nullable: true),
                    BKI_Physical = table.Column<int>(type: "int", nullable: true),
                    BKI_Self = table.Column<int>(type: "int", nullable: true),
                    BKI_Ati = table.Column<int>(type: "int", nullable: true),
                    BKI_Ekhtiar = table.Column<int>(type: "int", nullable: true),
                    BKI_Total_Value = table.Column<int>(type: "int", nullable: true),
                    BEI_Physical = table.Column<int>(type: "int", nullable: true),
                    BEI_Moshtaghe = table.Column<int>(type: "int", nullable: true),
                    BEI_Other = table.Column<int>(type: "int", nullable: true),
                    BEI_Total_Value = table.Column<int>(type: "int", nullable: true),
                    All_Total_Value = table.Column<int>(type: "int", nullable: true),
                    Date_Monthly = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction_Statistics_M", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brokerages");

            migrationBuilder.DropTable(
                name: "Transaction_Statistics_M");
        }
    }
}
