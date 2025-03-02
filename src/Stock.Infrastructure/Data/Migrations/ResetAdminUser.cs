using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Data.Migrations
{
    public partial class ResetAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tüm admin kullanıcılarını sil
            migrationBuilder.Sql(@"
                DELETE FROM ""Users"" 
                WHERE ""Username"" IN ('admin', 'Admin');
            ");

            // Yeni admin kullanıcısı ekle
            migrationBuilder.Sql($@"
                INSERT INTO ""Users"" (""Username"", ""PasswordHash"", ""IsAdmin"", ""CreatedAt"", ""UpdatedAt"", ""IsDeleted"")
                VALUES ('admin', '{BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123", 11)}', true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemi gerekmiyor
        }
    }
} 