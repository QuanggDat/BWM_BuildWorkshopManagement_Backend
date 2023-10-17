using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WokerTask_ManagerTask_ManagerTaskid",
                table: "WokerTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WokerTask_Order_orderId",
                table: "WokerTask");

            migrationBuilder.DropIndex(
                name: "IX_WokerTask_orderId",
                table: "WokerTask");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "WokerTask");

            migrationBuilder.RenameColumn(
                name: "ManagerTaskid",
                table: "WokerTask",
                newName: "managerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_WokerTask_ManagerTaskid",
                table: "WokerTask",
                newName: "IX_WokerTask_managerTaskId");

            migrationBuilder.AlterColumn<Guid>(
                name: "managerTaskId",
                table: "WokerTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WokerTask_ManagerTask_managerTaskId",
                table: "WokerTask",
                column: "managerTaskId",
                principalTable: "ManagerTask",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WokerTask_ManagerTask_managerTaskId",
                table: "WokerTask");

            migrationBuilder.RenameColumn(
                name: "managerTaskId",
                table: "WokerTask",
                newName: "ManagerTaskid");

            migrationBuilder.RenameIndex(
                name: "IX_WokerTask_managerTaskId",
                table: "WokerTask",
                newName: "IX_WokerTask_ManagerTaskid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerTaskid",
                table: "WokerTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "orderId",
                table: "WokerTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WokerTask_orderId",
                table: "WokerTask",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WokerTask_ManagerTask_ManagerTaskid",
                table: "WokerTask",
                column: "ManagerTaskid",
                principalTable: "ManagerTask",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WokerTask_Order_orderId",
                table: "WokerTask",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
