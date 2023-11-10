using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class WorkshopManagementSystem_BWM_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "varchar(350)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    member = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.id);
                });

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
                name: "MaterialCategory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    groupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    skill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    banStatus = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_roleId",
                        column: x => x.roleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Group_groupId",
                        column: x => x.groupId,
                        principalTable: "Group",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    length = table.Column<double>(type: "float", nullable: false),
                    depth = table.Column<double>(type: "float", nullable: false),
                    height = table.Column<double>(type: "float", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mass = table.Column<double>(type: "float", nullable: false),
                    drawingsTechnical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    drawings2D = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    drawings3D = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.id);
                    table.ForeignKey(
                        name: "FK_Item_ItemCategory_itemCategoryId",
                        column: x => x.itemCategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    materialCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thickness = table.Column<double>(type: "float", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    importPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.id);
                    table.ForeignKey(
                        name: "FK_Material_MaterialCategory_materialCategoryId",
                        column: x => x.materialCategoryId,
                        principalTable: "MaterialCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureStep",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assignToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inProgressTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quoteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    acceptanceTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fileContract = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileQuote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_assignToId",
                        column: x => x.assignToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_createdById",
                        column: x => x.createdById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProcedureItem_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureItem_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemMaterial",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    materialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMaterial", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItemMaterial_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemMaterial_Material_materialId",
                        column: x => x.materialId,
                        principalTable: "Material",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    leaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    itemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    drawingsTechnical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    procedureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaderTask_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LeaderTask_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LeaderTask_Procedure_procedureId",
                        column: x => x.procedureId,
                        principalTable: "Procedure",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    leaderTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reportType = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reportStatus = table.Column<int>(type: "int", nullable: true),
                    responseContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.id);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_reporterId",
                        column: x => x.reporterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_LeaderTask_leaderTaskId",
                        column: x => x.leaderTaskId,
                        principalTable: "LeaderTask",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Report_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerTask",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    leaderTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_WorkerTask_LeaderTask_leaderTaskId",
                        column: x => x.leaderTaskId,
                        principalTable: "LeaderTask",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerTask_Step_stepId",
                        column: x => x.stepId,
                        principalTable: "Step",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.id);
                    table.ForeignKey(
                        name: "FK_Resource_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Resource_Report_reportId",
                        column: x => x.reportId,
                        principalTable: "Report",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Supply",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    materialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supply", x => x.id);
                    table.ForeignKey(
                        name: "FK_Supply_Report_reportId",
                        column: x => x.reportId,
                        principalTable: "Report",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    leaderTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    workerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    seen = table.Column<bool>(type: "bit", nullable: false),
                    type = table.Column<int>(type: "int", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_LeaderTask_leaderTaskId",
                        column: x => x.leaderTaskId,
                        principalTable: "LeaderTask",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notification_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notification_Report_reportId",
                        column: x => x.reportId,
                        principalTable: "Report",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notification_WorkerTask_workerTaskId",
                        column: x => x.workerTaskId,
                        principalTable: "WorkerTask",
                        principalColumn: "id");
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_groupId",
                table: "AspNetUsers",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_roleId",
                table: "AspNetUsers",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Item_itemCategoryId",
                table: "Item",
                column: "itemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaterial_itemId",
                table: "ItemMaterial",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaterial_materialId",
                table: "ItemMaterial",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_createById",
                table: "LeaderTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderTask_itemId",
                table: "LeaderTask",
                column: "itemId");

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
                name: "IX_Material_materialCategoryId",
                table: "Material",
                column: "materialCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_leaderTaskId",
                table: "Notification",
                column: "leaderTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_orderId",
                table: "Notification",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_reportId",
                table: "Notification",
                column: "reportId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId",
                table: "Notification",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_workerTaskId",
                table: "Notification",
                column: "workerTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_assignToId",
                table: "Order",
                column: "assignToId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_createdById",
                table: "Order",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_itemId",
                table: "OrderDetail",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_orderId",
                table: "OrderDetail",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureItem_itemId",
                table: "ProcedureItem",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureItem_procedureId",
                table: "ProcedureItem",
                column: "procedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureStep_procedureId",
                table: "ProcedureStep",
                column: "procedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureStep_stepId",
                table: "ProcedureStep",
                column: "stepId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_leaderTaskId",
                table: "Report",
                column: "leaderTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_orderId",
                table: "Report",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_reporterId",
                table: "Report",
                column: "reporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_orderId",
                table: "Resource",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_reportId",
                table: "Resource",
                column: "reportId");

            migrationBuilder.CreateIndex(
                name: "IX_Supply_reportId",
                table: "Supply",
                column: "reportId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_createById",
                table: "WorkerTask",
                column: "createById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_leaderTaskId",
                table: "WorkerTask",
                column: "leaderTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_stepId",
                table: "WorkerTask",
                column: "stepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTaskDetail_userId",
                table: "WorkerTaskDetail",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTaskDetail_workerTaskId",
                table: "WorkerTaskDetail",
                column: "workerTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ItemMaterial");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ProcedureItem");

            migrationBuilder.DropTable(
                name: "ProcedureStep");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Supply");

            migrationBuilder.DropTable(
                name: "WorkerTaskDetail");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "WorkerTask");

            migrationBuilder.DropTable(
                name: "MaterialCategory");

            migrationBuilder.DropTable(
                name: "LeaderTask");

            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
