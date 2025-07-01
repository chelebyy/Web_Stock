-- Mevcut tabloları listele
SELECT tablename FROM pg_tables WHERE schemaname = 'public';

-- Permission tablosunun içeriğini kontrol et
SELECT * FROM "Permission" LIMIT 5;

-- RolePermission tablosunun içeriğini kontrol et
SELECT * FROM "RolePermission" LIMIT 5;

-- UserPermission tablosunun içeriğini kontrol et
SELECT * FROM "UserPermission" LIMIT 5; 