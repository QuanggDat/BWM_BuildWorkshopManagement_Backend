using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Report_reportId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "estimatedEndTime",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "estimatedStartTime",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "ProcedureStep");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "ProcedureStep");

            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "ProcedureItem");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "ProcedureItem");

            migrationBuilder.DropColumn(
                name: "endTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "estimatedEndTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "estimatedStartTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "estimatedEndTime",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "estimatedStartTime",
                table: "LeaderTask");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "WorkerTask",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "reportId",
                table: "Resource",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "orderId",
                table: "Resource",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "startTime",
                table: "Order",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "itemName",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "orderName",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_orderId",
                table: "Resource",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Order_orderId",
                table: "Resource",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Report_reportId",
                table: "Resource",
                column: "reportId",
                principalTable: "Report",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Order_orderId",
                table: "Resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Report_reportId",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_orderId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "itemName",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "orderName",
                table: "LeaderTask");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "WorkerTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedEndTime",
                table: "WorkerTask",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedStartTime",
                table: "WorkerTask",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "reportId",
                table: "Resource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "estimatedCompletedTime",
                table: "ProcedureStep",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "ProcedureStep",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "estimatedCompletedTime",
                table: "ProcedureItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "ProcedureItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "startTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "endTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedEndTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedStartTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedEndTime",
                table: "LeaderTask",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "estimatedStartTime",
                table: "LeaderTask",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Report_reportId",
                table: "Resource",
                column: "reportId",
                principalTable: "Report",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
