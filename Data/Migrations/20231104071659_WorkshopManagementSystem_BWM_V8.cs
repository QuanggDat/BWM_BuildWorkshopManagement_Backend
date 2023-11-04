using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "Procedure");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "Procedure");

            migrationBuilder.RenameColumn(
                name: "quoteDate",
                table: "Order",
                newName: "quoteTime");

            migrationBuilder.RenameColumn(
                name: "createDate",
                table: "Order",
                newName: "estimatedStartTime");

            migrationBuilder.RenameColumn(
                name: "acceptanceDate",
                table: "Order",
                newName: "acceptanceTime");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "createTime",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "ProcedureItem");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "ProcedureItem");

            migrationBuilder.DropColumn(
                name: "createTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "estimatedEndTime",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "quoteTime",
                table: "Order",
                newName: "quoteDate");

            migrationBuilder.RenameColumn(
                name: "estimatedStartTime",
                table: "Order",
                newName: "createDate");

            migrationBuilder.RenameColumn(
                name: "acceptanceTime",
                table: "Order",
                newName: "acceptanceDate");

            migrationBuilder.AddColumn<int>(
                name: "estimatedCompletedTime",
                table: "Procedure",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "Procedure",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
