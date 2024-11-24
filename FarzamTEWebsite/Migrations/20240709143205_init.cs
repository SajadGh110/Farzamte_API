using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HappyCalls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CallFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statusReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    introduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChoosingBrokerage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExplanationClub = table.Column<bool>(type: "bit", nullable: true),
                    UserRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    checkingPanel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirementDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirement1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirementDesc1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirement2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRequirementDesc2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeStatusAffter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTradeAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalBrokerCommission = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HappyCalls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HappyCalls");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
