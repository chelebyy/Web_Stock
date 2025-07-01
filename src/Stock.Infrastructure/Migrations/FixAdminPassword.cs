using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Users\" SET \"PasswordHash\" = '$2a$11$3J5..wF0T4iP.gR2Y5tA5.L/E.CHd2/x3O5aZl4Y3k58S.s/3x8ge' WHERE \"Sicil\" = '00000'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Users\" SET \"PasswordHash\" = '' WHERE \"Sicil\" = '00000'"); // Geri alma adımı
        }
    }
} 