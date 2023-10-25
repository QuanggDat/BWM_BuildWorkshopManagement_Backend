using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_MaterialCategory_categoryId",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_categoryId",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Notification",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "twoD",
                table: "Item",
                newName: "drawingsTechnical");

            migrationBuilder.RenameColumn(
                name: "threeD",
                table: "Item",
                newName: "drawings3D");

            migrationBuilder.RenameColumn(
                name: "technical",
                table: "Item",
                newName: "drawings2D");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialCategoryid",
                table: "Material",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Material_MaterialCategoryid",
                table: "Material",
                column: "MaterialCategoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material",
                column: "MaterialCategoryid",
                principalTable: "MaterialCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_MaterialCategoryid",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "MaterialCategoryid",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Notification",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "drawingsTechnical",
                table: "Item",
                newName: "twoD");

            migrationBuilder.RenameColumn(
                name: "drawings3D",
                table: "Item",
                newName: "threeD");

            migrationBuilder.RenameColumn(
                name: "drawings2D",
                table: "Item",
                newName: "technical");

            migrationBuilder.CreateIndex(
                name: "IX_Material_categoryId",
                table: "Material",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_categoryId",
                table: "Material",
                column: "categoryId",
                principalTable: "MaterialCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
