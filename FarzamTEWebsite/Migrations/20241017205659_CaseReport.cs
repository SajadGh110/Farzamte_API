using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class CaseReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statuscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Broker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    casetype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    caseAutoNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phonecallReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phonecallReasonsDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseResolutionCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CaseResolutionSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseResolutionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseResolutionsolver = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseReports");
        }
    }
}
