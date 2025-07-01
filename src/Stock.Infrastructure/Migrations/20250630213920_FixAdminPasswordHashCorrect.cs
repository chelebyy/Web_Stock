using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminPasswordHashCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Admin kullanıcısının password hash'ini güncelle (Sicil: 00000)
            migrationBuilder.Sql(@"
                UPDATE ""Users"" 
                SET ""PasswordHash"" = '$2a$11$cY5cjBXnBwufNioFza4wwOI8YqvBhCPn1biy0vgx4RgSeGTlsYgRG' 
                WHERE ""Sicil_Value"" = '00000';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eski password hash'ini geri yükle
            migrationBuilder.Sql(@"
                UPDATE ""Users"" 
                SET ""PasswordHash"" = '$2a$11$3J5..wF0T4iP.gR2Y5tA5.L/E.CHd2/x3O5aZl4Y3k58S.s/3x8ge' 
                WHERE ""Sicil_Value"" = '00000';
            ");
        }
    }
}
