using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "member",
                table: "Group");

            migrationBuilder.AddColumn<string>(
                name: "feedbackContent",
                table: "WorkerTaskDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "feedbackTitle",
                table: "WorkerTaskDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "WorkerTaskDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "workerTaskDetailId",
                table: "Resource",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_workerTaskDetailId",
                table: "Resource",
                column: "workerTaskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_WorkerTaskDetail_workerTaskDetailId",
                table: "Resource",
                column: "workerTaskDetailId",
                principalTable: "WorkerTaskDetail",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_WorkerTaskDetail_workerTaskDetailId",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_workerTaskDetailId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "feedbackContent",
                table: "WorkerTaskDetail");

            migrationBuilder.DropColumn(
                name: "feedbackTitle",
                table: "WorkerTaskDetail");

            migrationBuilder.DropColumn(
                name: "status",
                table: "WorkerTaskDetail");

            migrationBuilder.DropColumn(
                name: "workerTaskDetailId",
                table: "Resource");

            migrationBuilder.AddColumn<int>(
                name: "member",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
