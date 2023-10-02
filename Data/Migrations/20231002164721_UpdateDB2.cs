using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdateDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connect1_Item_itemId",
                table: "Connect1");

            migrationBuilder.DropForeignKey(
                name: "FK_Connect1_Material_materialId",
                table: "Connect1");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connect1",
                table: "Connect1");

            migrationBuilder.RenameTable(
                name: "Connect1",
                newName: "ItemMaterial");

            migrationBuilder.RenameColumn(
                name: "materialId",
                table: "Material",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Connect1_materialId",
                table: "ItemMaterial",
                newName: "IX_ItemMaterial_materialId");

            migrationBuilder.RenameIndex(
                name: "IX_Connect1_itemId",
                table: "ItemMaterial",
                newName: "IX_ItemMaterial_itemId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Order",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialCategoryid",
                table: "Material",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Item",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemCategorycategoryId",
                table: "Item",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "categoryId",
                table: "Item",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Floor",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Area",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemMaterial",
                table: "ItemMaterial",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(1000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TaskType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ItemProcedure",
                columns: table => new
                {
                    Itemsid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Proceduresid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProcedure", x => new { x.Itemsid, x.Proceduresid });
                    table.ForeignKey(
                        name: "FK_ItemProcedure_Item_Itemsid",
                        column: x => x.Itemsid,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemProcedure_Procedure_Proceduresid",
                        column: x => x.Proceduresid,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    timeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productCompleted = table.Column<int>(type: "int", nullable: false),
                    productFailed = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    taskTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.id);
                    table.ForeignKey(
                        name: "FK_Task_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_TaskType_taskTypeId",
                        column: x => x.taskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskUser",
                columns: table => new
                {
                    Tasksid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskUser", x => new { x.Tasksid, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TaskUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskUser_Task_Tasksid",
                        column: x => x.Tasksid,
                        principalTable: "Task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_itemId",
                table: "OrderDetail",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProcedure_Proceduresid",
                table: "ItemProcedure",
                column: "Proceduresid");

            migrationBuilder.CreateIndex(
                name: "IX_Task_orderId",
                table: "Task",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_taskTypeId",
                table: "Task",
                column: "taskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUser_UsersId",
                table: "TaskUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                table: "Item",
                column: "ItemCategorycategoryId",
                principalTable: "ItemCategory",
                principalColumn: "categoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMaterial_Item_itemId",
                table: "ItemMaterial",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMaterial_Material_materialId",
                table: "ItemMaterial",
                column: "materialId",
                principalTable: "Material",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material",
                column: "MaterialCategoryid",
                principalTable: "MaterialCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMaterial_Item_itemId",
                table: "ItemMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMaterial_Material_materialId",
                table: "ItemMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material");

            migrationBuilder.DropTable(
                name: "ItemProcedure");

            migrationBuilder.DropTable(
                name: "TaskUser");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_itemId",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemMaterial",
                table: "ItemMaterial");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Floor");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Area");

            migrationBuilder.RenameTable(
                name: "ItemMaterial",
                newName: "Connect1");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Material",
                newName: "materialId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMaterial_materialId",
                table: "Connect1",
                newName: "IX_Connect1_materialId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMaterial_itemId",
                table: "Connect1",
                newName: "IX_Connect1_itemId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Order",
                type: "nvarchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialCategoryid",
                table: "Material",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Item",
                type: "nvarchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemCategorycategoryId",
                table: "Item",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "itemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connect1",
                table: "Connect1",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Connect1_Item_itemId",
                table: "Connect1",
                column: "itemId",
                principalTable: "Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connect1_Material_materialId",
                table: "Connect1",
                column: "materialId",
                principalTable: "Material",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                table: "Item",
                column: "ItemCategorycategoryId",
                principalTable: "ItemCategory",
                principalColumn: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_MaterialCategory_MaterialCategoryid",
                table: "Material",
                column: "MaterialCategoryid",
                principalTable: "MaterialCategory",
                principalColumn: "id");
        }
    }
}
