using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_WokerTask_wokerTaskId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "WokerTaskDetail");

            migrationBuilder.DropTable(
                name: "WokerTask");

            migrationBuilder.RenameColumn(
                name: "wokerTaskId",
                table: "Notification",
                newName: "workerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_wokerTaskId",
                table: "Notification",
                newName: "IX_Notification_workerTaskId");

            migrationBuilder.CreateTable(
                name: "WorkerTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    managerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTask", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkerTask_AspNetUsers_createById",
                        column: x => x.createById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerTask_ManagerTask_managerTaskId",
                        column: x => x.managerTaskId,
                        principalTable: "ManagerTask",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTaskDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    productCompleted = table.Column<int>(type: "int", nullable: true),
                    productFailed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTaskDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkerTaskDetail_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerTaskDetail_WorkerTask_workerTaskId",
                        column: x => x.workerTaskId,
                        principalTable: "WorkerTask",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_createById",
                table: "WorkerTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_managerTaskId",
                table: "WorkerTask",
                column: "managerTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTaskDetail_userId",
                table: "WorkerTaskDetail",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTaskDetail_workerTaskId",
                table: "WorkerTaskDetail",
                column: "workerTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_WorkerTask_workerTaskId",
                table: "Notification",
                column: "workerTaskId",
                principalTable: "WorkerTask",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_WorkerTask_workerTaskId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "WorkerTaskDetail");

            migrationBuilder.DropTable(
                name: "WorkerTask");

            migrationBuilder.RenameColumn(
                name: "workerTaskId",
                table: "Notification",
                newName: "wokerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_workerTaskId",
                table: "Notification",
                newName: "IX_Notification_wokerTaskId");

            migrationBuilder.CreateTable(
                name: "WokerTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    managerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WokerTask", x => x.id);
                    table.ForeignKey(
                        name: "FK_WokerTask_AspNetUsers_createById",
                        column: x => x.createById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WokerTask_ManagerTask_managerTaskId",
                        column: x => x.managerTaskId,
                        principalTable: "ManagerTask",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WokerTaskDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    wokerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    productCompleted = table.Column<int>(type: "int", nullable: true),
                    productFailed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WokerTaskDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_WokerTaskDetail_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WokerTaskDetail_WokerTask_wokerTaskId",
                        column: x => x.wokerTaskId,
                        principalTable: "WokerTask",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WokerTask_createById",
                table: "WokerTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_WokerTask_managerTaskId",
                table: "WokerTask",
                column: "managerTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WokerTaskDetail_userId",
                table: "WokerTaskDetail",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_WokerTaskDetail_wokerTaskId",
                table: "WokerTaskDetail",
                column: "wokerTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_WokerTask_wokerTaskId",
                table: "Notification",
                column: "wokerTaskId",
                principalTable: "WokerTask",
                principalColumn: "id");
        }
    }
}
