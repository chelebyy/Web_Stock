using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixActivityLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ActivityLogs tablosundaki CreatedBy ve UpdatedBy alanlarını nullable yap
            migrationBuilder.Sql(@"
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""CreatedBy"" DROP NOT NULL;
                
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UpdatedBy"" DROP NOT NULL;
                
                -- UserId alanını nullable yap ve foreign key kısıtlamasını kaldır
                ALTER TABLE ""ActivityLogs"" 
                DROP CONSTRAINT ""FK_ActivityLogs_Users_UserId"";
                
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UserId"" DROP NOT NULL;
                
                ALTER TABLE ""ActivityLogs"" 
                ADD CONSTRAINT ""FK_ActivityLogs_Users_UserId"" 
                FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") 
                ON DELETE SET NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Değişiklikleri geri al (NOT NULL kısıtlamasını geri ekle)
            // Not: Bu işlem mevcut NULL değerler varsa başarısız olabilir
            migrationBuilder.Sql(@"
                -- UserId alanını tekrar NOT NULL yap ve foreign key kısıtlamasını geri ekle
                ALTER TABLE ""ActivityLogs"" 
                DROP CONSTRAINT ""FK_ActivityLogs_Users_UserId"";
                
                UPDATE ""ActivityLogs"" 
                SET ""UserId"" = 1 
                WHERE ""UserId"" IS NULL;
                
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UserId"" SET NOT NULL;
                
                ALTER TABLE ""ActivityLogs"" 
                ADD CONSTRAINT ""FK_ActivityLogs_Users_UserId"" 
                FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") 
                ON DELETE CASCADE;
                
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""CreatedBy"" SET NOT NULL;
                
                ALTER TABLE ""ActivityLogs"" 
                ALTER COLUMN ""UpdatedBy"" SET NOT NULL;
            ");
        }
    }
}
