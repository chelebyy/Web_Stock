-- =============================================================================
-- Stock Application Seed Data Generation Script
-- =============================================================================
-- Bu script, performans analizi için veritabanını anlamlı verilerle doldurur.
-- v2: NOT NULL kısıtlamalarına uyması için CreatedAt, Description, IsDeleted
--     gibi alanlar eklendi.
-- =============================================================================

BEGIN;

-- 1. Rolleri Ekle
INSERT INTO "Roles" ("Id", "Name", "Description", "CreatedAt", "CreatedBy", "IsDeleted") VALUES
(1, 'Admin', 'Administrator role with full access', NOW(), 'System', false),
(2, 'IT_Manager', 'IT Manager role for IT inventory management', NOW(), 'System', false),
(3, 'Infirmary_Manager', 'Infirmary Manager role for medical inventory management', NOW(), 'System', false),
(4, 'Standard_User', 'Standard User with basic permissions', NOW(), 'System', false)
ON CONFLICT ("Id") DO NOTHING;

-- 2. Kategorileri Ekle
INSERT INTO "Categories" ("Id", "Name", "Description", "CreatedAt", "CreatedBy", "IsDeleted") VALUES
(1, 'Bilgisayarlar', 'Dizüstü, masaüstü bilgisayarlar ve sunucular.', NOW(), 'System', false),
(2, 'Monitörler', 'Tüm boyut ve tiplerde monitörler.', NOW(), 'System', false),
(3, 'Klavye & Mouse', 'Kablolu ve kablosuz klavye ve mouse setleri.', NOW(), 'System', false),
(4, 'Yazıcılar', 'Lazer, mürekkep püskürtmeli ve termal yazıcılar.', NOW(), 'System', false),
(5, 'Ağ Ürünleri', 'Router, switch, modem ve ağ kabloları.', NOW(), 'System', false),
(6, 'Tıbbi Cihazlar', 'Revirde kullanılan medikal cihazlar.', NOW(), 'System', false),
(7, 'Sarf Malzemeler', 'Yazıcı toneri, kağıt, pil gibi ofis ve tıbbi sarf malzemeleri.', NOW(), 'System', false)
ON CONFLICT ("Id") DO NOTHING;

-- Sequence'leri güncelle (elle ID verildiği için)
SELECT setval(pg_get_serial_sequence('"Roles"', 'Id'), COALESCE(max("Id"), 1), max("Id") IS NOT null) FROM "Roles";
SELECT setval(pg_get_serial_sequence('"Categories"', 'Id'), COALESCE(max("Id"), 1), max("Id") IS NOT null) FROM "Categories";


-- 3. Kullanıcıları Ekle
-- Not: Şifre hash'i bir placeholder'dır.
-- v2: Adi ve Soyadi sütunları eklendi, IsAdmin alanı dolduruldu.
INSERT INTO "Users" ("Id", "Adi", "Soyadi", "Sicil", "PasswordHash", "IsAdmin", "RoleId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(1, 10),
    'Admin',
    'User ' || generate_series(1, 10),
    'A' || lpad(generate_series(1, 10)::text, 5, '0'),
    'AQAAAAIAAYagAAAAEEj4j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3',
    true, -- IsAdmin
    1, -- Admin Rolü
    NOW(),
    'System',
    false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Users" ("Id", "Adi", "Soyadi", "Sicil", "PasswordHash", "IsAdmin", "RoleId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(11, 20),
    'IT',
    'User ' || generate_series(1, 10),
    'IT' || lpad(generate_series(1, 10)::text, 4, '0'),
    'AQAAAAIAAYagAAAAEEj4j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3',
    false, -- IsAdmin
    2, -- IT Manager Rolü
    NOW(),
    'System',
    false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Users" ("Id", "Adi", "Soyadi", "Sicil", "PasswordHash", "IsAdmin", "RoleId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(21, 200),
    'Standard',
    'User ' || generate_series(1, 180),
    'U' || lpad(generate_series(1, 180)::text, 5, '0'),
    'AQAAAAIAAYagAAAAEEj4j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3j4H3',
    false, -- IsAdmin
    4, -- Standard User Rolü
    NOW(),
    'System',
    false
ON CONFLICT ("Id") DO NOTHING;

SELECT setval(pg_get_serial_sequence('"Users"', 'Id'), COALESCE(max("Id"), 1), max("Id") IS NOT null) FROM "Users";


-- 4. Ürünleri Ekle
-- v2: StockQuantity ve IsDeleted sütunları eklendi, CriticalStock kaldırıldı.
INSERT INTO "Products" ("Id", "Name", "Description", "StockQuantity", "CategoryId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(1, 250),
    'Laptop Model ' || (random() * 1000)::int,
    'General purpose laptop for office use. Model No: ' || (random() * 1000)::int,
    (random() * 100)::int,
    1, -- Bilgisayarlar
    NOW(), 'System', false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Products" ("Id", "Name", "Description", "StockQuantity", "CategoryId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(251, 400),
    'Monitor 24" Model ' || (random() * 500)::int,
    '24 inch Full HD office monitor. Model No: ' || (random() * 500)::int,
    (random() * 50)::int,
    2, -- Monitörler
    NOW(), 'System', false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Products" ("Id", "Name", "Description", "StockQuantity", "CategoryId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(401, 700),
    'Keyboard/Mouse Set ' || (random() * 200)::int,
    'Wireless keyboard and mouse combo set. Model No: ' || (random() * 200)::int,
    (random() * 200)::int,
    3, -- Klavye & Mouse
    NOW(), 'System', false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Products" ("Id", "Name", "Description", "StockQuantity", "CategoryId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(701, 750),
    'Laser Printer ' || (random() * 100)::int,
    'Monochrome laser printer for office use. Model No: ' || (random() * 100)::int,
    (random() * 20)::int,
    4, -- Yazıcılar
    NOW(), 'System', false
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Products" ("Id", "Name", "Description", "StockQuantity", "CategoryId", "CreatedAt", "CreatedBy", "IsDeleted")
SELECT
    generate_series(751, 1000),
    'Cat6 Cable ' || (random() * 100)::int || 'm',
    'UTP Cat6 ethernet network cable.',
    (random() * 500)::int,
    5, -- Ağ Ürünleri
    NOW(), 'System', false
ON CONFLICT ("Id") DO NOTHING;


SELECT setval(pg_get_serial_sequence('"Products"', 'Id'), COALESCE(max("Id"), 1), max("Id") IS NOT null) FROM "Products";

-- Not: Permissions, UserPermissions ve RolePermissions tabloları uygulamanın kendisi
-- veya ayrı bir script tarafından yönetildiği için bu seed script'ine dahil edilmemiştir.
-- Analiz için temel CRUD ve listeleme sorguları bu verilerle yapılabilir.

COMMIT; 