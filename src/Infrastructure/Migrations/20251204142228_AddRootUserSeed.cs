using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRootUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Run", "CreatedAt", "CreatedByUserRun", "DeletedAt", "DeletedByUserRun", "Dv", "Email", "FirstNames", "IsActive", "IsDeleted", "LastNames", "UpdatedAt", "UpdatedByUserRun" },
                values: new object[] { 19241027, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "4", "jonathan.d.navarro.v@gmail.com", "Jonathan Damián", true, false, "Navarro Vega", new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Run",
                keyValue: 19241027);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}
