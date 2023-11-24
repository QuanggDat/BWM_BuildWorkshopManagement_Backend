using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "materialColor",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materialName",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materialSku",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materialSupplier",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "materialThickness",
                table: "Supply",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "materialUnit",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materialColor",
                table: "OrderDetailMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materialUnit",
                table: "OrderDetailMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updateTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "materialColor",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialName",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialSku",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialSupplier",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialThickness",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialUnit",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialColor",
                table: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "materialUnit",
                table: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "updateTime",
                table: "Order");
        }
    }
}
