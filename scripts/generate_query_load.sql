-- =============================================================================
-- Stock Application Query Load Generation Script
-- =============================================================================
-- Bu script, performans analizi için veritabanında gerçekçi bir okuma
-- (SELECT) yükü oluşturur.
-- pg_stat_statements tarafından analiz edilecek sorguları üretir.
--
-- KULLANIM TALİMATI:
-- Bu script'i veritabanı yönetim aracınızda BİRKAÇ KEZ (en az 5-10 defa)
-- çalıştırın. Ne kadar çok çalıştırırsanız, istatistikler o kadar anlamlı olur.
-- =============================================================================

-- -- -- KULLANICI YÖNETİMİ SORGULARI -- -- --

-- 1. Tüm kullanıcıları listeleme (sayfalama ile - ilk sayfa)
SELECT "Id", "Adi", "Soyadi", "Sicil", "RoleId" FROM "Users" WHERE "IsDeleted" = false ORDER BY "Adi" OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-- 2. Tüm kullanıcıları listeleme (sayfalama ile - rastgele bir sayfa)
SELECT "Id", "Adi", "Soyadi", "Sicil", "RoleId" FROM "Users" WHERE "IsDeleted" = false ORDER BY "Adi" OFFSET 50 ROWS FETCH NEXT 10 ROWS ONLY;

-- 3. İsme göre kullanıcı arama
SELECT "Id", "Adi", "Soyadi", "Sicil" FROM "Users" WHERE "Adi" ILIKE '%Standard%' AND "IsDeleted" = false;

-- 4. Belirli bir role ait kullanıcıları getirme (Admin rolü)
SELECT "Id", "Adi", "Soyadi", "Sicil" FROM "Users" WHERE "RoleId" = 1 AND "IsDeleted" = false;

-- 5. Tek bir kullanıcı detayını getirme
SELECT * FROM "Users" WHERE "Id" = 100;


-- -- -- ÜRÜN YÖNETİMİ SORGULARI -- -- --

-- 6. Tüm ürünleri ve kategori isimlerini listeleme (sayfalama ile - ilk sayfa)
SELECT p."Id", p."Name", p."StockQuantity", c."Name" as "CategoryName"
FROM "Products" p
JOIN "Categories" c ON p."CategoryId" = c."Id"
WHERE p."IsDeleted" = false
ORDER BY p."Name"
OFFSET 0 ROWS FETCH NEXT 25 ROWS ONLY;

-- 7. Tüm ürünleri ve kategori isimlerini listeleme (sayfalama ile - rastgele bir sayfa)
SELECT p."Id", p."Name", p."StockQuantity", c."Name" as "CategoryName"
FROM "Products" p
JOIN "Categories" c ON p."CategoryId" = c."Id"
WHERE p."IsDeleted" = false
ORDER BY p."Name"
OFFSET 200 ROWS FETCH NEXT 25 ROWS ONLY;

-- 8. Ürün ismine göre arama
SELECT p."Id", p."Name", p."StockQuantity" FROM "Products" p WHERE p."Name" ILIKE '%Laptop%' AND p."IsDeleted" = false;

-- 9. Belirli bir kategoriye ait ürünleri listeleme (Monitörler)
SELECT p."Id", p."Name", p."StockQuantity" FROM "Products" p WHERE p."CategoryId" = 2 AND p."IsDeleted" = false;

-- 10. Stok miktarı kritik seviyenin altında olan ürünleri bulma (Simülasyon)
-- Not: CriticalStock sütunu olmadığı için StockQuantity üzerinden bir eşik belirliyoruz.
SELECT p."Id", p."Name", p."StockQuantity" FROM "Products" p WHERE p."StockQuantity" < 10 AND p."IsDeleted" = false;

-- 11. Tek bir ürün detayını getirme
SELECT * FROM "Products" WHERE "Id" = 500;

-- -- -- ROL VE KATEGORİ SORGULARI -- -- --

-- 12. Tüm rolleri listeleme
SELECT "Id", "Name", "Description" FROM "Roles" WHERE "IsDeleted" = false;

-- 13. Tüm kategorileri listeleme
SELECT "Id", "Name", "Description" FROM "Categories" WHERE "IsDeleted" = false;

-- -- -- DASHBOARD SORGULARI (Simülasyon) -- -- --

-- 14. Toplam kullanıcı, ürün, kategori sayısı
SELECT
  (SELECT COUNT(*) FROM "Users" WHERE "IsDeleted" = false) as TotalUsers,
  (SELECT COUNT(*) FROM "Products" WHERE "IsDeleted" = false) as TotalProducts,
  (SELECT COUNT(*) FROM "Categories" WHERE "IsDeleted" = false) as TotalCategories;

-- 15. Kategori bazında ürün sayısı
SELECT c."Name", COUNT(p."Id") as ProductCount
FROM "Products" p
JOIN "Categories" c ON p."CategoryId" = c."Id"
WHERE p."IsDeleted" = false
GROUP BY c."Name"
ORDER BY ProductCount DESC; 