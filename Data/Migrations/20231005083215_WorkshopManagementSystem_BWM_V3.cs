using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_AspNetUsers_userId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Nest_nestId",
                table: "Group");

            migrationBuilder.DropTable(
                name: "Nest");

            migrationBuilder.DropIndex(
                name: "IX_Group_nestId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "nestId",
                table: "Group");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Group",
                newName: "squadId");

            migrationBuilder.RenameIndex(
                name: "IX_Group_userId",
                table: "Group",
                newName: "IX_Group_squadId");

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    Groupid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.Groupid, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Group_Groupid",
                        column: x => x.Groupid,
                        principalTable: "Group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Squad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Squad", x => x.id);
                    table.ForeignKey(
                        name: "FK_Squad_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UserId",
                table: "GroupUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Squad_UserId",
                table: "Squad",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Squad_squadId",
                table: "Group",
                column: "squadId",
                principalTable: "Squad",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Squad_squadId",
                table: "Group");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Squad");

            migrationBuilder.RenameColumn(
                name: "squadId",
                table: "Group",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Group_squadId",
                table: "Group",
                newName: "IX_Group_userId");

            migrationBuilder.AddColumn<Guid>(
                name: "nestId",
                table: "Group",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Nest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nest", x => x.id);
                    table.ForeignKey(
                        name: "FK_Nest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_nestId",
                table: "Group",
                column: "nestId");

            migrationBuilder.CreateIndex(
                name: "IX_Nest_UserId",
                table: "Nest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_AspNetUsers_userId",
                table: "Group",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Nest_nestId",
                table: "Group",
                column: "nestId",
                principalTable: "Nest",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
