using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_AspNetUsers_leaderId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Team_teamId",
                table: "LeaderTask");

            migrationBuilder.RenameColumn(
                name: "teamId",
                table: "LeaderTask",
                newName: "Teamid");

            migrationBuilder.RenameIndex(
                name: "IX_LeaderTask_teamId",
                table: "LeaderTask",
                newName: "IX_LeaderTask_Teamid");

            migrationBuilder.AddColumn<Guid>(
                name: "stepId",
                table: "WorkerTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "leaderId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "itemId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "LeaderTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_stepId",
                table: "WorkerTask",
                column: "stepId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_itemId",
                table: "LeaderTask",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_AspNetUsers_leaderId",
                table: "LeaderTask",
                column: "leaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Team_Teamid",
                table: "LeaderTask",
                column: "Teamid",
                principalTable: "Team",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_Step_stepId",
                table: "WorkerTask",
                column: "stepId",
                principalTable: "Step",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_AspNetUsers_leaderId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Team_Teamid",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_Step_stepId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_WorkerTask_stepId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_LeaderTask_itemId",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "stepId",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "name",
                table: "LeaderTask");

            migrationBuilder.RenameColumn(
                name: "Teamid",
                table: "LeaderTask",
                newName: "teamId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaderTask_Teamid",
                table: "LeaderTask",
                newName: "IX_LeaderTask_teamId");

            migrationBuilder.AlterColumn<Guid>(
                name: "leaderId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_AspNetUsers_leaderId",
                table: "LeaderTask",
                column: "leaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Team_teamId",
                table: "LeaderTask",
                column: "teamId",
                principalTable: "Team",
                principalColumn: "id");
        }
    }
}
