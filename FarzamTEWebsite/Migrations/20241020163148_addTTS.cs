using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class addTTS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportsToSmart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Broker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customerSatisfaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resultOfCall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportsToSmart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TTS_Reasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransportToSmartId = table.Column<int>(type: "int", nullable: true),
                    TransportToSmartId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTS_Reasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TTS_Reasons_TransportsToSmart_TransportToSmartId",
                        column: x => x.TransportToSmartId,
                        principalTable: "TransportsToSmart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TTS_Reasons_TransportsToSmart_TransportToSmartId1",
                        column: x => x.TransportToSmartId1,
                        principalTable: "TransportsToSmart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TTS_Reasons_TransportToSmartId",
                table: "TTS_Reasons",
                column: "TransportToSmartId");

            migrationBuilder.CreateIndex(
                name: "IX_TTS_Reasons_TransportToSmartId1",
                table: "TTS_Reasons",
                column: "TransportToSmartId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TTS_Reasons");

            migrationBuilder.DropTable(
                name: "TransportsToSmart");
        }
    }
}
