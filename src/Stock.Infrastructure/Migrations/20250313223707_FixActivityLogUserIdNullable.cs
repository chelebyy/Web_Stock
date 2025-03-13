using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixActivityLogUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // UserId alanını nullable yap ve foreign key kısıtlamasını güncelle
            migrationBuilder.Sql(@"
                -- Foreign key kısıtlamasını kaldır
                ALTER TABLE ""ActivityLogs"" 
                DROP CONSTRAINT ""FK_ActivityLogs_Users_UserId"";
                
                -- UserId alanını nullable yap
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UserId"" DROP NOT NULL;
                
                -- Foreign key kısıtlamasını tekrar ekle (ON DELETE SET NULL ile)
                ALTER TABLE ""ActivityLogs"" 
                ADD CONSTRAINT ""FK_ActivityLogs_Users_UserId"" 
                FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") 
                ON DELETE SET NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Değişiklikleri geri al
            migrationBuilder.Sql(@"
                -- Foreign key kısıtlamasını kaldır
                ALTER TABLE ""ActivityLogs"" 
                DROP CONSTRAINT ""FK_ActivityLogs_Users_UserId"";
                
                -- NULL değerleri varsayılan admin kullanıcısına ata (ID=1)
                UPDATE ""ActivityLogs"" 
                SET ""UserId"" = 1 
                WHERE ""UserId"" IS NULL;
                
                -- UserId alanını tekrar NOT NULL yap
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UserId"" SET NOT NULL;
                
                -- Foreign key kısıtlamasını tekrar ekle (ON DELETE CASCADE ile)
                ALTER TABLE ""ActivityLogs"" 
                ADD CONSTRAINT ""FK_ActivityLogs_Users_UserId"" 
                FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") 
                ON DELETE CASCADE;
            ");
        }
    }
}
