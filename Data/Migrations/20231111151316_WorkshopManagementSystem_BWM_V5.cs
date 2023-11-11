using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "materialName",
                table: "Supply");

            migrationBuilder.AddColumn<Guid>(
                name: "materialId",
                table: "Supply",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Supply",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Supply_materialId",
                table: "Supply",
                column: "materialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supply_Material_materialId",
                table: "Supply",
                column: "materialId",
                principalTable: "Material",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supply_Material_materialId",
                table: "Supply");

            migrationBuilder.DropIndex(
                name: "IX_Supply_materialId",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "materialId",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Supply");

            migrationBuilder.AddColumn<string>(
                name: "materialName",
                table: "Supply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
