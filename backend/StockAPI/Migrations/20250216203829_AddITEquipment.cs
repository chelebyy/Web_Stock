using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StockAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddITEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ITEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AssignedTo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEquipments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ITEquipments_SerialNumber",
                table: "ITEquipments",
                column: "SerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITEquipments");
        }
    }
}
