using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Step_Procedure_procedureId",
                table: "Step");

            migrationBuilder.DropIndex(
                name: "IX_Step_procedureId",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "estimatedCompletedTime",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "procedureId",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "importDate",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "totalPrice",
                table: "Material");

            migrationBuilder.CreateTable(
                name: "ProcedureStep",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    estimatedCompletedTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureStep", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProcedureStep_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureStep_Step_stepId",
                        column: x => x.stepId,
                        principalTable: "Step",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureStep_procedureId",
                table: "ProcedureStep",
                column: "procedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureStep_stepId",
                table: "ProcedureStep",
                column: "stepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcedureStep");

            migrationBuilder.AddColumn<int>(
                name: "estimatedCompletedTime",
                table: "Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "procedureId",
                table: "Step",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "amount",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "importDate",
                table: "Material",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "totalPrice",
                table: "Material",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Step_procedureId",
                table: "Step",
                column: "procedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Procedure_procedureId",
                table: "Step",
                column: "procedureId",
                principalTable: "Procedure",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
