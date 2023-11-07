using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_teamId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Team_Teamid",
                table: "LeaderTask");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_LeaderTask_Teamid",
                table: "LeaderTask");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_teamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teamid",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "orderName",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "teamId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Teamid",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "orderName",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "teamId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    groupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    member = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.id);
                    table.ForeignKey(
                        name: "FK_Team_Group_groupId",
                        column: x => x.groupId,
                        principalTable: "Group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_Teamid",
                table: "LeaderTask",
                column: "Teamid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_teamId",
                table: "AspNetUsers",
                column: "teamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_groupId",
                table: "Team",
                column: "groupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Team_teamId",
                table: "AspNetUsers",
                column: "teamId",
                principalTable: "Team",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Team_Teamid",
                table: "LeaderTask",
                column: "Teamid",
                principalTable: "Team",
                principalColumn: "id");
        }
    }
}
