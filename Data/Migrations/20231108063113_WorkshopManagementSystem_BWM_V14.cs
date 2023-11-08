using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Area_Areaid",
                table: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Floor");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_Areaid",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "drawingsTechnical",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "Areaid",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "procedureId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "itemId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "amount",
                table: "LeaderTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask",
                column: "procedureId",
                principalTable: "Procedure",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "LeaderTask");

            migrationBuilder.AddColumn<string>(
                name: "drawingsTechnical",
                table: "WorkerTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Areaid",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "procedureId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "itemId",
                table: "LeaderTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    name = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    floorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    name = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.id);
                    table.ForeignKey(
                        name: "FK_Area_Floor_floorId",
                        column: x => x.floorId,
                        principalTable: "Floor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Areaid",
                table: "OrderDetail",
                column: "Areaid");

            migrationBuilder.CreateIndex(
                name: "IX_Area_floorId",
                table: "Area",
                column: "floorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Item_itemId",
                table: "LeaderTask",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderTask_Procedure_procedureId",
                table: "LeaderTask",
                column: "procedureId",
                principalTable: "Procedure",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Area_Areaid",
                table: "OrderDetail",
                column: "Areaid",
                principalTable: "Area",
                principalColumn: "id");
        }
    }
}
