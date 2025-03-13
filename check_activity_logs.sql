-- ActivityLogs tablosunun yapısını kontrol et
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'ActivityLogs'
ORDER BY ordinal_position;

-- ActivityLogs tablosunun foreign key ilişkilerini kontrol et
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
  AND tc.table_name = 'ActivityLogs'
ORDER BY tc.table_name, kcu.column_name;

-- ActivityLogs tablosundaki verileri kontrol et
SELECT * FROM "ActivityLogs" LIMIT 10;

-- Users tablosuyla birleştirerek kontrol et
SELECT 
    a.*, 
    u."Username" AS UserName
FROM 
    "ActivityLogs" a
    LEFT JOIN "Users" u ON a."UserId" = u."Id"
LIMIT 10; 