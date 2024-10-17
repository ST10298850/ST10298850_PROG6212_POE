using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10298850_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class ChangingCoordinatorTableToAddDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InteractionDate",
                table: "Coordinators",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractionDate",
                table: "Coordinators");
        }
    }
}
