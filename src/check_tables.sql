-- Tablo adlarını listele
SELECT tablename FROM pg_tables WHERE schemaname = 'public';

-- Permission tablosundaki verileri kontrol et
SELECT * FROM "Permission" LIMIT 5;

-- RolePermission tablosundaki verileri kontrol et
SELECT * FROM "RolePermission" LIMIT 5;

-- UserPermission tablosundaki verileri kontrol et
SELECT * FROM "UserPermission" LIMIT 5; 