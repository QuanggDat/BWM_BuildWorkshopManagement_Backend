using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productCompleted",
                table: "WorkerTaskDetail");

            migrationBuilder.DropColumn(
                name: "productFailed",
                table: "WorkerTaskDetail");

            migrationBuilder.AddColumn<int>(
                name: "productCompleted",
                table: "LeaderTask",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productFailed",
                table: "LeaderTask",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productCompleted",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "productFailed",
                table: "LeaderTask");

            migrationBuilder.AddColumn<int>(
                name: "productCompleted",
                table: "WorkerTaskDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productFailed",
                table: "WorkerTaskDetail",
                type: "int",
                nullable: true);
        }
    }
}
