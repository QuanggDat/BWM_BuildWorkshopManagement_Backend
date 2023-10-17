using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "orderId",
                table: "Report",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_orderId",
                table: "Report",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Order_orderId",
                table: "Report",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Order_orderId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_orderId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "Report");
        }
    }
}
