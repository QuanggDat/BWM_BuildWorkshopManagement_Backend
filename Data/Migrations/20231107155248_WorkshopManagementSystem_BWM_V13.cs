using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Area_areaId",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "areaId",
                table: "OrderDetail",
                newName: "Areaid");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_areaId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_Areaid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Areaid",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Area_Areaid",
                table: "OrderDetail",
                column: "Areaid",
                principalTable: "Area",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Area_Areaid",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "Areaid",
                table: "OrderDetail",
                newName: "areaId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_Areaid",
                table: "OrderDetail",
                newName: "IX_OrderDetail_areaId");

            migrationBuilder.AlterColumn<Guid>(
                name: "areaId",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Area_areaId",
                table: "OrderDetail",
                column: "areaId",
                principalTable: "Area",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
