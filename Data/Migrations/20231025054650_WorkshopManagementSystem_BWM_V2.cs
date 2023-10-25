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
                table: "WokerTask");

            migrationBuilder.DropColumn(
                name: "productFailed",
                table: "WokerTask");

            migrationBuilder.RenameColumn(
                name: "dateCreated",
                table: "Notification",
                newName: "createdDate");

            migrationBuilder.AddColumn<int>(
                name: "productCompleted",
                table: "WokerTaskDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productFailed",
                table: "WokerTaskDetail",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productCompleted",
                table: "WokerTaskDetail");

            migrationBuilder.DropColumn(
                name: "productFailed",
                table: "WokerTaskDetail");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                table: "Notification",
                newName: "dateCreated");

            migrationBuilder.AddColumn<int>(
                name: "productCompleted",
                table: "WokerTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "productFailed",
                table: "WokerTask",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
