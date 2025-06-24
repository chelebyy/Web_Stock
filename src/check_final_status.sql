-- Mevcut tabloları listele
SELECT tablename FROM pg_tables WHERE schemaname = 'public';

-- Permissions tablosunun içeriğini kontrol et
SELECT * FROM "Permissions" LIMIT 5;

-- RolePermissions tablosunun içeriğini kontrol et
SELECT * FROM "RolePermissions";

-- UserPermissions tablosunun içeriğini kontrol et
SELECT * FROM "UserPermissions"; 