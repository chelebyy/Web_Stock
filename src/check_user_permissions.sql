-- UserPermissions tablosunun yapısını kontrol et
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'UserPermissions'
ORDER BY ordinal_position;

-- UserPermissions tablosunun foreign key ilişkilerini kontrol et
SELECT
    tc.table_schema, 
    tc.constraint_name, 
    tc.table_name, 
    kcu.column_name, 
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM 
    information_schema.table_constraints AS tc 
    JOIN information_schema.key_column_usage AS kcu
      ON tc.constraint_name = kcu.constraint_name
      AND tc.table_schema = kcu.table_schema
    JOIN information_schema.constraint_column_usage AS ccu
      ON ccu.constraint_name = tc.constraint_name
      AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_name = 'UserPermissions'
ORDER BY tc.table_name, kcu.column_name;

-- UserPermissions tablosundaki verileri kontrol et
SELECT * FROM "UserPermissions" LIMIT 10;

-- Users ve Permissions tablolarıyla birleştirerek kontrol et
SELECT 
    up.*, 
    u.Username, 
    p.Name AS PermissionName
FROM 
    "UserPermissions" up
    LEFT JOIN "Users" u ON up."UserId" = u."Id"
    LEFT JOIN "Permissions" p ON up."PermissionId" = p."Id"
LIMIT 10; 