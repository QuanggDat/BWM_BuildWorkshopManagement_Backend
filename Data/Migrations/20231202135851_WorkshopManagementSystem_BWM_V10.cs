using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "groupId",
                table: "Log",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "materialId",
                table: "Log",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_groupId",
                table: "Log",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_materialId",
                table: "Log",
                column: "materialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Group_groupId",
                table: "Log",
                column: "groupId",
                principalTable: "Group",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Material_materialId",
                table: "Log",
                column: "materialId",
                principalTable: "Material",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Group_groupId",
                table: "Log");

            migrationBuilder.DropForeignKey(
                name: "FK_Log_Material_materialId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_groupId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_materialId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "groupId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "materialId",
                table: "Log");
        }
    }
}
