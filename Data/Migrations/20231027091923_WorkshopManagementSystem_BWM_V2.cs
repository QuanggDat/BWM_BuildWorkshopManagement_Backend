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
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "MaterialCategoryid",
                table: "Material",
                newName: "materialCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Material_MaterialCategoryid",
                table: "Material",
                newName: "IX_Material_materialCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "image",
                table: "Material",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "Material",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_materialCategoryId",
                table: "Material",
                column: "materialCategoryId",
                principalTable: "MaterialCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_MaterialCategory_materialCategoryId",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "materialCategoryId",
                table: "Material",
                newName: "MaterialCategoryid");

            migrationBuilder.RenameIndex(
                name: "IX_Material_materialCategoryId",
                table: "Material",
                newName: "IX_Material_MaterialCategoryid");

            migrationBuilder.AlterColumn<string>(
                name: "image",
                table: "Material",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "Material",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "categoryId",
                table: "Material",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material",
                column: "MaterialCategoryid",
                principalTable: "MaterialCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
