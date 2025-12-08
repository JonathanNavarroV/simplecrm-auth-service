using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PermissionModuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionSections_PermissionModules_PermissionModuleId",
                        column: x => x.PermissionModuleId,
                        principalTable: "PermissionModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PermissionModules",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "USERS", "Gestión de usuarios y autenticación", true, "Usuarios" });

            migrationBuilder.InsertData(
                table: "PermissionTypes",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "CREATE", "Permiso para crear recursos", true, "Creación" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "READ", "Permiso para leer recursos", true, "Lectura" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "WRITE", "Permiso para modificar recursos", true, "Escritura" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "DELETE", "Permiso para eliminar recursos", true, "Eliminación" }
                });

            migrationBuilder.InsertData(
                table: "PermissionSections",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name", "PermissionModuleId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "USERS", "Sección para gestión de usuarios", true, "Usuarios", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "ROLES", "Sección para gestión de roles", true, "Roles", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "PERMISSIONS", "Sección para gestión de permisos", true, "Permisos", new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionModules_Code",
                table: "PermissionModules",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSections_PermissionModuleId",
                table: "PermissionSections",
                column: "PermissionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTypes_Code",
                table: "PermissionTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionSections");

            migrationBuilder.DropTable(
                name: "PermissionTypes");

            migrationBuilder.DropTable(
                name: "PermissionModules");
        }
    }
}
