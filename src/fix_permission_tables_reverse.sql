-- 1. Önce yeni tabloların içeriğini eski tablolara taşıyalım
-- Permission tablosundaki verileri Permissions tablosuna taşı
INSERT INTO "Permissions" ("Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "Permission"
WHERE NOT EXISTS (
    SELECT 1 FROM "Permissions" p WHERE p."Id" = "Permission"."Id"
);

-- RolePermission tablosundaki verileri RolePermissions tablosuna taşı
INSERT INTO "RolePermissions" ("Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "RolePermission"
WHERE NOT EXISTS (
    SELECT 1 FROM "RolePermissions" rp WHERE rp."Id" = "RolePermission"."Id"
);

-- UserPermission tablosundaki verileri UserPermissions tablosuna taşı
INSERT INTO "UserPermissions" ("Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "UserPermission"
WHERE NOT EXISTS (
    SELECT 1 FROM "UserPermissions" up WHERE up."Id" = "UserPermission"."Id"
);

-- 2. ApplicationDbContext.cs dosyasını güncelleyin ve DbSet'leri doğru tablo adlarına göre ayarlayın
-- Bu dosyada aşağıdaki değişiklikleri yapın:
-- public DbSet<Stock.Domain.Entities.Permissions.Permission> Permissions { get; set; } = null!;
-- public DbSet<Stock.Domain.Entities.Permissions.RolePermission> RolePermissions { get; set; } = null!;
-- public DbSet<Stock.Domain.Entities.Permissions.UserPermission> UserPermissions { get; set; } = null!;

-- 3. Yeni tabloları kaldır (CASCADE ile bağımlılıkları da kaldır)
-- DROP TABLE IF EXISTS "Permission" CASCADE;
-- DROP TABLE IF EXISTS "RolePermission" CASCADE;
-- DROP TABLE IF EXISTS "UserPermission" CASCADE; 