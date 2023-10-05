using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Nest_nestId",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "nestId",
                table: "Report",
                newName: "managerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_nestId",
                table: "Report",
                newName: "IX_Report_managerTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ManagerTask_managerTaskId",
                table: "Report",
                column: "managerTaskId",
                principalTable: "ManagerTask",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_ManagerTask_managerTaskId",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "managerTaskId",
                table: "Report",
                newName: "nestId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_managerTaskId",
                table: "Report",
                newName: "IX_Report_nestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Nest_nestId",
                table: "Report",
                column: "nestId",
                principalTable: "Nest",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
