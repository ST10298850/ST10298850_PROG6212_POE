using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10298850_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddingNotesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Claims");
        }
    }
}
