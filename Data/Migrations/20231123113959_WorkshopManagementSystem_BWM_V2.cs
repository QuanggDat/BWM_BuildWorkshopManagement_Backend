using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "materiaThickness",
                table: "OrderDetailMaterial",
                newName: "materialThickness");

            migrationBuilder.RenameColumn(
                name: "materiaSupplier",
                table: "OrderDetailMaterial",
                newName: "materialSupplier");

            migrationBuilder.RenameColumn(
                name: "materiaSku",
                table: "OrderDetailMaterial",
                newName: "materialSku");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "materialThickness",
                table: "OrderDetailMaterial",
                newName: "materiaThickness");

            migrationBuilder.RenameColumn(
                name: "materialSupplier",
                table: "OrderDetailMaterial",
                newName: "materiaSupplier");

            migrationBuilder.RenameColumn(
                name: "materialSku",
                table: "OrderDetailMaterial",
                newName: "materiaSku");
        }
    }
}
