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
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_Step_stepId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_WorkerTask_stepId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_LeaderTask_procedureId",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "stepId",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "procedureId",
                table: "LeaderTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "stepId",
                table: "WorkerTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "procedureId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_stepId",
                table: "WorkerTask",
                column: "stepId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_procedureId",
                table: "LeaderTask",
                column: "procedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask",
                column: "procedureId",
                principalTable: "Procedure",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_Step_stepId",
                table: "WorkerTask",
                column: "stepId",
                principalTable: "Step",
                principalColumn: "id");
        }
    }
}
