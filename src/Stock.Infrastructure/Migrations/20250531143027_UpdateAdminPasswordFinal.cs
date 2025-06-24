using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Users\" SET \"PasswordHash\" = '$2a$12$1mQ8COT0UtZQkwP5cKKw6eHqKfQyzEXvJvrFXWzBMkrrLX3qUADZu' WHERE \"Sicil\" = '00000'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Orijinal hash bilinmediği için, Down metodu şimdilik bir değişiklik yapmayacak
            // veya bilinen güvenli bir duruma (örn: null veya geçici bir hash) döndürebilir.
            // migrationBuilder.Sql("UPDATE \"Users\" SET \"PasswordHash\" = 'SOME_DEFAULT_OR_NULL_HASH' WHERE \"Sicil\" = '00000'");
        }
    }
}
