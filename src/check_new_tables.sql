-- Mevcut tabloları listele
SELECT tablename FROM pg_tables WHERE schemaname = 'public';

-- Permissions tablosunun içeriğini kontrol et
SELECT * FROM "Permissions" LIMIT 5;

-- RolePermissions tablosunun içeriğini kontrol et
SELECT * FROM "RolePermissions" LIMIT 5;

-- UserPermissions tablosunun içeriğini kontrol et
SELECT * FROM "UserPermissions" LIMIT 5; 