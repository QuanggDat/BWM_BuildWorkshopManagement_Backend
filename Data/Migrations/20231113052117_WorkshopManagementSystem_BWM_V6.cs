using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "productFailed",
                table: "LeaderTask",
                newName: "itemFailed");

            migrationBuilder.RenameColumn(
                name: "productCompleted",
                table: "LeaderTask",
                newName: "itemCompleted");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "LeaderTask",
                newName: "itemQuantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "itemQuantity",
                table: "LeaderTask",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "itemFailed",
                table: "LeaderTask",
                newName: "productFailed");

            migrationBuilder.RenameColumn(
                name: "itemCompleted",
                table: "LeaderTask",
                newName: "productCompleted");
        }
    }
}
