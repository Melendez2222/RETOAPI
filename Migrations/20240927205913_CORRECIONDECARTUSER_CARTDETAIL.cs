﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETOAPI.Migrations
{
    /// <inheritdoc />
    public partial class CORRECIONDECARTUSER_CARTDETAIL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "CartUser");

            migrationBuilder.DropColumn(
                name: "PriceCD",
                table: "CartDetail");

            migrationBuilder.DropColumn(
                name: "QuantityCD",
                table: "CartDetail");

            migrationBuilder.DropColumn(
                name: "SubtotalCD",
                table: "CartDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "CartUser",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceCD",
                table: "CartDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QuantityCD",
                table: "CartDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SubtotalCD",
                table: "CartDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
