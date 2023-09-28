using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ItemAndMaterialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "MaterialCategory",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCategory", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    mass = table.Column<double>(type: "float", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    length = table.Column<double>(type: "float", nullable: false),
                    width = table.Column<double>(type: "float", nullable: false),
                    height = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    ItemCategorycategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.id);
                    table.ForeignKey(
                        name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                        column: x => x.ItemCategorycategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "categoryId");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    materialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    sku = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    importDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    importPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thickness = table.Column<double>(type: "float", nullable: false),
                    supplier = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MaterialCategorycategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.materialId);
                    table.ForeignKey(
                        name: "FK_Material_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Material_MaterialCategory_MaterialCategorycategoryId",
                        column: x => x.MaterialCategorycategoryId,
                        principalTable: "MaterialCategory",
                        principalColumn: "categoryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemCategorycategoryId",
                table: "Item",
                column: "ItemCategorycategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_itemId",
                table: "Material",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_MaterialCategorycategoryId",
                table: "Material",
                column: "MaterialCategorycategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "MaterialCategory");

            migrationBuilder.DropTable(
                name: "ItemCategory");
        }
    }
}
