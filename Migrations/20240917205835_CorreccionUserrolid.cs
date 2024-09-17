using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETOAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionUserrolid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolID",
                table: "Users");
        }
    }
}
