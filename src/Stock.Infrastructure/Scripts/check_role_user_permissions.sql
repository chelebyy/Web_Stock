-- RolePermission tablosundaki verileri kontrol et
SELECT * FROM "RolePermission";

-- UserPermission tablosundaki verileri kontrol et
SELECT * FROM "UserPermission";

-- Şimdi bu verileri RolePermissions ve UserPermissions tablolarına taşıyalım
INSERT INTO "RolePermissions" ("Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "RolePermission"
WHERE NOT EXISTS (
    SELECT 1 FROM "RolePermissions" rp WHERE rp."Id" = "RolePermission"."Id"
);

INSERT INTO "UserPermissions" ("Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "UserPermission"
WHERE NOT EXISTS (
    SELECT 1 FROM "UserPermissions" up WHERE up."Id" = "UserPermission"."Id"
); 