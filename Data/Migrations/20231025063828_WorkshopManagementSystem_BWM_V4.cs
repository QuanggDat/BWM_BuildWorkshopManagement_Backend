using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeStart",
                table: "WokerTask",
                newName: "startTime");

            migrationBuilder.RenameColumn(
                name: "timeEnd",
                table: "WokerTask",
                newName: "endTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startTime",
                table: "WokerTask",
                newName: "timeStart");

            migrationBuilder.RenameColumn(
                name: "endTime",
                table: "WokerTask",
                newName: "timeEnd");
        }
    }
}
