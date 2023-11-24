using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "itemCategoryName",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemCode",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "itemDepth",
                table: "OrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "itemDrawings2D",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemDrawings3D",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemDrawingsTechnical",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "itemHeight",
                table: "OrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "itemImage",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "itemLength",
                table: "OrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "itemMass",
                table: "OrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "itemName",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemUnit",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "itemCategoryName",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemCode",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemDepth",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemDrawings2D",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemDrawings3D",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemDrawingsTechnical",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemHeight",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemImage",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemLength",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemMass",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemName",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "itemUnit",
                table: "OrderDetail");
        }
    }
}
