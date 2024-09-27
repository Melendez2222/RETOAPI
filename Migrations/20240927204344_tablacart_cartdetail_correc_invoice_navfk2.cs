using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETOAPI.Migrations
{
    /// <inheritdoc />
    public partial class tablacart_cartdetail_correc_invoice_navfk2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Invoice_InvoiceId1",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_InvoiceId1",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "InvoiceId1",
                table: "Invoice");

            migrationBuilder.CreateTable(
                name: "CartUser",
                columns: table => new
                {
                    IdCart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartUser", x => x.IdCart);
                    table.ForeignKey(
                        name: "FK_CartUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetail",
                columns: table => new
                {
                    IdItemCart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCart = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    QuantityCD = table.Column<int>(type: "int", nullable: false),
                    PriceCD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubtotalCD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetail", x => x.IdItemCart);
                    table.ForeignKey(
                        name: "FK_CartDetail_CartUser_IdCart",
                        column: x => x.IdCart,
                        principalTable: "CartUser",
                        principalColumn: "IdCart",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id_Product",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_IdCart",
                table: "CartDetail",
                column: "IdCart");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_ProductId",
                table: "CartDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartUser_UserId",
                table: "CartUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetail");

            migrationBuilder.DropTable(
                name: "CartUser");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId1",
                table: "Invoice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceId1",
                table: "Invoice",
                column: "InvoiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Invoice_InvoiceId1",
                table: "Invoice",
                column: "InvoiceId1",
                principalTable: "Invoice",
                principalColumn: "InvoiceId");
        }
    }
}
