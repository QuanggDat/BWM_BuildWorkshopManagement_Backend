using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    orderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.id);
                    table.ForeignKey(
                        name: "FK_Log_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Log_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Log_OrderDetail_orderDetailId",
                        column: x => x.orderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_orderDetailId",
                table: "Log",
                column: "orderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_orderId",
                table: "Log",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_userId",
                table: "Log",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
