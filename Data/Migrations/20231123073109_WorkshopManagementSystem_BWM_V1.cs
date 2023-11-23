using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "materiaSku",
                table: "OrderDetailMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "materiaSupplier",
                table: "OrderDetailMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "materiaThickness",
                table: "OrderDetailMaterial",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "materialName",
                table: "OrderDetailMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "materiaSku",
                table: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "materiaSupplier",
                table: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "materiaThickness",
                table: "OrderDetailMaterial");

            migrationBuilder.DropColumn(
                name: "materialName",
                table: "OrderDetailMaterial");
        }
    }
}
