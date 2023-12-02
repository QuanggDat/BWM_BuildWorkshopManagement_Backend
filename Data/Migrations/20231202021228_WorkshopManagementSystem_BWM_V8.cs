using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "itemId",
                table: "Log",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_itemId",
                table: "Log",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Item_itemId",
                table: "Log",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Item_itemId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_itemId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "Log");
        }
    }
}
