using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "action",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "dateUpdated",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Notification",
                newName: "title");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "orderId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_orderId",
                table: "Notification",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Order_orderId",
                table: "Notification",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Order_orderId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_orderId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "type",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Notification",
                newName: "description");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateUpdated",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
