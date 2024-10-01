using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETOAPI.Migrations
{
    /// <inheritdoc />
    public partial class Correciondecartuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_CartUser_IdCart",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CartUser_Users_UserId",
                table: "CartUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartUser",
                table: "CartUser");

            migrationBuilder.RenameTable(
                name: "CartUser",
                newName: "CartUsers");

            migrationBuilder.RenameIndex(
                name: "IX_CartUser_UserId",
                table: "CartUsers",
                newName: "IX_CartUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartUsers",
                table: "CartUsers",
                column: "IdCart");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_CartUsers_IdCart",
                table: "CartDetail",
                column: "IdCart",
                principalTable: "CartUsers",
                principalColumn: "IdCart",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartUsers_Users_UserId",
                table: "CartUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_CartUsers_IdCart",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CartUsers_Users_UserId",
                table: "CartUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartUsers",
                table: "CartUsers");

            migrationBuilder.RenameTable(
                name: "CartUsers",
                newName: "CartUser");

            migrationBuilder.RenameIndex(
                name: "IX_CartUsers_UserId",
                table: "CartUser",
                newName: "IX_CartUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartUser",
                table: "CartUser",
                column: "IdCart");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_CartUser_IdCart",
                table: "CartDetail",
                column: "IdCart",
                principalTable: "CartUser",
                principalColumn: "IdCart",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartUser_Users_UserId",
                table: "CartUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
