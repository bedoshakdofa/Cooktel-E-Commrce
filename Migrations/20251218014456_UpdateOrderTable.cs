using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cooktel_E_commrece.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
