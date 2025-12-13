using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByType = table.Column<int>(type: "integer", nullable: false),
                    CreatedBySystemName = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    CreatedByUserRun = table.Column<int>(type: "integer", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedByUserRole = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EntityType = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    EntityDisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EntityId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    BeforeState = table.Column<string>(type: "jsonb", nullable: true),
                    AfterState = table.Column<string>(type: "jsonb", nullable: true),
                    Changes = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CreatedAt",
                table: "EventLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CreatedByUserRun",
                table: "EventLogs",
                column: "CreatedByUserRun");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_EntityType_EntityId_CreatedAt",
                table: "EventLogs",
                columns: new[] { "EntityType", "EntityId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogs");
        }
    }
}
