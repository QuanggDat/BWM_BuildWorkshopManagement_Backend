using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeStart",
                table: "ManagerTask",
                newName: "startTime");

            migrationBuilder.RenameColumn(
                name: "timeEnd",
                table: "ManagerTask",
                newName: "endTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startTime",
                table: "ManagerTask",
                newName: "timeStart");

            migrationBuilder.RenameColumn(
                name: "endTime",
                table: "ManagerTask",
                newName: "timeEnd");
        }
    }
}
