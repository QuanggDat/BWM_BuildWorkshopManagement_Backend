using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "drawings2D",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "drawings3D",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "drawingsTechnical",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "itemName",
                table: "LeaderTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "drawings2D",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "drawings3D",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "drawingsTechnical",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemName",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
