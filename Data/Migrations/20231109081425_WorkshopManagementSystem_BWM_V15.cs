using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "roleID",
                table: "AspNetUsers",
                newName: "roleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_roleID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_roleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleId",
                table: "AspNetUsers",
                column: "roleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "roleId",
                table: "AspNetUsers",
                newName: "roleID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_roleId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_roleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleID",
                table: "AspNetUsers",
                column: "roleID",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
