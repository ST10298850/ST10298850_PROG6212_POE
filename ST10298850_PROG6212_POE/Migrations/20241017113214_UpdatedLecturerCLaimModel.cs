using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10298850_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLecturerCLaimModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CoordinatorId",
                table: "Claims",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CoordinatorId",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
