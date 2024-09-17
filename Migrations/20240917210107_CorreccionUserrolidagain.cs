using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETOAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionUserrolidagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rols_RolId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RolId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RolID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RolId1",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolId1",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId1",
                table: "Users",
                column: "RolId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rols_RolId1",
                table: "Users",
                column: "RolId1",
                principalTable: "Rols",
                principalColumn: "RolId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
