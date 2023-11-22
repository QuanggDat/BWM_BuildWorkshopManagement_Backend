using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "OrderDetail");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OrderDetailMaterial",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    materialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailMaterial", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDetailMaterial_Material_materialId",
                        column: x => x.materialId,
                        principalTable: "Material",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetailMaterial_OrderDetail_orderDetailId",
                        column: x => x.orderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailMaterial_materialId",
                table: "OrderDetailMaterial",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailMaterial_orderDetailId",
                table: "OrderDetailMaterial",
                column: "orderDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Report");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "OrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
