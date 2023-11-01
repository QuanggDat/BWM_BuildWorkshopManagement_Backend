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
                name: "FK_AspNetUsers_Squad_squadId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Squad_squadId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_AspNetUsers_createById",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_AspNetUsers_createdById",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMaterial_AspNetUsers_createdById",
                table: "ItemMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_AspNetUsers_createById",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialCategory_AspNetUsers_createById",
                table: "MaterialCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_ManagerTask_managerTaskId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_ManagerTask_managerTaskId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_ManagerTask_managerTaskId",
                table: "WorkerTask");

            migrationBuilder.DropTable(
                name: "ManagerTask");

            migrationBuilder.DropTable(
                name: "Squad");

            migrationBuilder.DropIndex(
                name: "IX_MaterialCategory_createById",
                table: "MaterialCategory");

            migrationBuilder.DropIndex(
                name: "IX_Material_createById",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_ItemMaterial_createdById",
                table: "ItemMaterial");

            migrationBuilder.DropIndex(
                name: "IX_Item_createById",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Group_squadId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "createById",
                table: "MaterialCategory");

            migrationBuilder.DropColumn(
                name: "createById",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "ItemMaterial");

            migrationBuilder.DropColumn(
                name: "createById",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "squadId",
                table: "Group");

            migrationBuilder.RenameColumn(
                name: "managerTaskId",
                table: "WorkerTask",
                newName: "leaderTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_managerTaskId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_leaderTaskId");

            migrationBuilder.RenameColumn(
                name: "managerTaskId",
                table: "Report",
                newName: "leaderTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_managerTaskId",
                table: "Report",
                newName: "IX_Report_leaderTaskId");

            migrationBuilder.RenameColumn(
                name: "managerTaskId",
                table: "Notification",
                newName: "leaderTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_managerTaskId",
                table: "Notification",
                newName: "IX_Notification_leaderTaskId");

            migrationBuilder.RenameColumn(
                name: "createdById",
                table: "Item",
                newName: "itemCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_createdById",
                table: "Item",
                newName: "IX_Item_itemCategoryId");

            migrationBuilder.RenameColumn(
                name: "squadId",
                table: "AspNetUsers",
                newName: "teamId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_squadId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_teamId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "MaterialCategory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Item",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    groupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    member = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "LeaderTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    leaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    teamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderTask", x => x.id);
                    table.ForeignKey(
                        name: "FK_LeaderTask_AspNetUsers_createById",
                        column: x => x.createById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaderTask_AspNetUsers_leaderId",
                        column: x => x.leaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderTask_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LeaderTask_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderTask_Team_teamId",
                        column: x => x.teamId,
                        principalTable: "Team",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_createById",
                table: "LeaderTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_leaderId",
                table: "LeaderTask",
                column: "leaderId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_orderId",
                table: "LeaderTask",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_procedureId",
                table: "LeaderTask",
                column: "procedureId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_teamId",
                table: "LeaderTask",
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
                name: "FK_Item_ItemCategory_itemCategoryId",
                table: "Item",
                column: "itemCategoryId",
                principalTable: "ItemCategory",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_LeaderTask_leaderTaskId",
                table: "Notification",
                column: "leaderTaskId",
                principalTable: "LeaderTask",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_LeaderTask_leaderTaskId",
                table: "Report",
                column: "leaderTaskId",
                principalTable: "LeaderTask",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_LeaderTask_leaderTaskId",
                table: "WorkerTask",
                column: "leaderTaskId",
                principalTable: "LeaderTask",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_teamId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemCategory_itemCategoryId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_LeaderTask_leaderTaskId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_LeaderTask_leaderTaskId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_LeaderTask_leaderTaskId",
                table: "WorkerTask");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "LeaderTask");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.RenameColumn(
                name: "leaderTaskId",
                table: "WorkerTask",
                newName: "managerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_leaderTaskId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_managerTaskId");

            migrationBuilder.RenameColumn(
                name: "leaderTaskId",
                table: "Report",
                newName: "managerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_leaderTaskId",
                table: "Report",
                newName: "IX_Report_managerTaskId");

            migrationBuilder.RenameColumn(
                name: "leaderTaskId",
                table: "Notification",
                newName: "managerTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_leaderTaskId",
                table: "Notification",
                newName: "IX_Notification_managerTaskId");

            migrationBuilder.RenameColumn(
                name: "itemCategoryId",
                table: "Item",
                newName: "createdById");

            migrationBuilder.RenameIndex(
                name: "IX_Item_itemCategoryId",
                table: "Item",
                newName: "IX_Item_createdById");

            migrationBuilder.RenameColumn(
                name: "teamId",
                table: "AspNetUsers",
                newName: "squadId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_teamId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_squadId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "MaterialCategory",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "createById",
                table: "MaterialCategory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "createById",
                table: "Material",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "createdById",
                table: "ItemMaterial",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Item",
                type: "nvarchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "createById",
                table: "Item",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "squadId",
                table: "Group",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ManagerTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    groupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    managerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerTask", x => x.id);
                    table.ForeignKey(
                        name: "FK_ManagerTask_AspNetUsers_createById",
                        column: x => x.createById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManagerTask_AspNetUsers_managerId",
                        column: x => x.managerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerTask_Group_groupId",
                        column: x => x.groupId,
                        principalTable: "Group",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ManagerTask_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ManagerTask_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Squad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    member = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Squad", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCategory_createById",
                table: "MaterialCategory",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_Material_createById",
                table: "Material",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaterial_createdById",
                table: "ItemMaterial",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_Item_createById",
                table: "Item",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_Group_squadId",
                table: "Group",
                column: "squadId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTask_createById",
                table: "ManagerTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTask_groupId",
                table: "ManagerTask",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTask_managerId",
                table: "ManagerTask",
                column: "managerId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTask_orderId",
                table: "ManagerTask",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTask_procedureId",
                table: "ManagerTask",
                column: "procedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Squad_squadId",
                table: "AspNetUsers",
                column: "squadId",
                principalTable: "Squad",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Squad_squadId",
                table: "Group",
                column: "squadId",
                principalTable: "Squad",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_AspNetUsers_createById",
                table: "Item",
                column: "createById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_AspNetUsers_createdById",
                table: "Item",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMaterial_AspNetUsers_createdById",
                table: "ItemMaterial",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_AspNetUsers_createById",
                table: "Material",
                column: "createById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialCategory_AspNetUsers_createById",
                table: "MaterialCategory",
                column: "createById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_ManagerTask_managerTaskId",
                table: "Notification",
                column: "managerTaskId",
                principalTable: "ManagerTask",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ManagerTask_managerTaskId",
                table: "Report",
                column: "managerTaskId",
                principalTable: "ManagerTask",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_ManagerTask_managerTaskId",
                table: "WorkerTask",
                column: "managerTaskId",
                principalTable: "ManagerTask",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
