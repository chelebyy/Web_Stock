﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSicilUniqueConstraintForActiveUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Sicil",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Sicil",
                table: "Users",
                column: "Sicil",
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Sicil",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Sicil",
                table: "Users",
                column: "Sicil",
                unique: true);
        }
    }
}
