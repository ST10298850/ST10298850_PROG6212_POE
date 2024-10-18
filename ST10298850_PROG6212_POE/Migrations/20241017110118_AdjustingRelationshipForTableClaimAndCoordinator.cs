﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10298850_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingRelationshipForTableClaimAndCoordinator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinatorId",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoordinatorId",
                table: "Claims",
                column: "CoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Coordinators_CoordinatorId",
                table: "Claims",
                column: "CoordinatorId",
                principalTable: "Coordinators",
                principalColumn: "CoordinatorId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Coordinators_CoordinatorId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_CoordinatorId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "CoordinatorId",
                table: "Claims");
        }
    }
}