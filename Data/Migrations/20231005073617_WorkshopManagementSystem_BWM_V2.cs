using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WokerTask_Group_Groupid",
                table: "WokerTask");

            migrationBuilder.DropIndex(
                name: "IX_WokerTask_Groupid",
                table: "WokerTask");

            migrationBuilder.DropColumn(
                name: "Groupid",
                table: "WokerTask");

            migrationBuilder.RenameColumn(
                name: "materialId",
                table: "Material",
                newName: "categoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Material",
                newName: "materialId");

            migrationBuilder.AddColumn<Guid>(
                name: "Groupid",
                table: "WokerTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WokerTask_Groupid",
                table: "WokerTask",
                column: "Groupid");

            migrationBuilder.AddForeignKey(
                name: "FK_WokerTask_Group_Groupid",
                table: "WokerTask",
                column: "Groupid",
                principalTable: "Group",
                principalColumn: "id");
        }
    }
}
