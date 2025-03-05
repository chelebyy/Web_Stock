using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Administrator with full access");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 2, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Normal user with limited access", "User", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Sicil" },
                values: new object[] { "$2a$11$KmueNQA.11/UVyH3q.r1pejRgkfyTJhQPr0D9HBD/79QkyVYFnIni", "A001" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "LastLoginAt", "PasswordHash", "RoleId", "Sicil", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 2, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "$2a$11$jH.Q1xRMYt1OBwdO8IvyGOkdUAMGOYaJYQJLg/DaKAt85XGQbEBba", 2, "U001", null, null, "user" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Administrator role with full access");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Sicil" },
                values: new object[] { "$2a$11$ij4DH2oHJIgakH3QZS94Uu1/jandJqMjWZtQB8B5.JoK4zrBd2Lhu", "" });
        }
    }
}
