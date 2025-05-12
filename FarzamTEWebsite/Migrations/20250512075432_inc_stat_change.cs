using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarzamTEWebsite.Migrations
{
    /// <inheritdoc />
    public partial class inc_stat_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "brokerage",
                table: "inComingCall_Stats",
                newName: "Broker");

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "inComingCall_Stats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Broker",
                table: "inComingCall_Stats",
                newName: "brokerage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Month",
                table: "inComingCall_Stats",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
