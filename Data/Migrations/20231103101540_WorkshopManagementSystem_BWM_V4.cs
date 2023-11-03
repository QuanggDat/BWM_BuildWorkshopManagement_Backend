using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "orderDate",
                table: "Order",
                newName: "startTime");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Procedure",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AddColumn<int>(
                name: "estimatedCompletedTime",
                table: "Procedure",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "Procedure",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "endTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    estimatedCompletedTime = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.id);
                    table.ForeignKey(
                        name: "FK_Step_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Step_procedureId",
                table: "Step",
                column: "procedureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "Procedure");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "Procedure");

            migrationBuilder.DropColumn(
                name: "createDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "endTime",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "startTime",
                table: "Order",
                newName: "orderDate");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Procedure",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
