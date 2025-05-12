using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class inc_stat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inComingCall_Stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    brokerage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Received = table.Column<int>(type: "int", nullable: false),
                    Answered = table.Column<int>(type: "int", nullable: false),
                    Unanswered = table.Column<int>(type: "int", nullable: false),
                    Abandoned = table.Column<int>(type: "int", nullable: false),
                    Transferred = table.Column<int>(type: "int", nullable: false),
                    Logins = table.Column<int>(type: "int", nullable: false),
                    Logoff = table.Column<int>(type: "int", nullable: false),
                    Avg_Wait = table.Column<int>(type: "int", nullable: false),
                    Avg_Talk = table.Column<int>(type: "int", nullable: false),
                    Max_Callers = table.Column<int>(type: "int", nullable: false),
                    Answ = table.Column<float>(type: "real", nullable: false),
                    Unansw = table.Column<float>(type: "real", nullable: false),
                    SLA = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inComingCall_Stats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inComingCall_Stats");
        }
    }
}
