$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupPath = ".\db_backup_before_netcore9_$timestamp.sql"
$dbname = "StockDB"
$username = "postgres"

Write-Host "Veritabanı yedeği alınıyor: $backupPath"

try {
    & 'C:\Program Files\PostgreSQL\17\bin\pg_dump.exe' -U $username -d $dbname -f $backupPath
    Write-Host "Veritabanı yedeği başarıyla alındı: $backupPath"
} catch {
    Write-Error "Veritabanı yedeği alınırken hata oluştu: $_"
    exit 1
} 