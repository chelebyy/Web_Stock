-- Önce yeni tabloları temizleyelim
TRUNCATE TABLE "Permissions" CASCADE;
TRUNCATE TABLE "RolePermissions" CASCADE;
TRUNCATE TABLE "UserPermissions" CASCADE;

-- Verileri eski tablolardan yeni tablolara taşıyalım
INSERT INTO "Permissions" ("Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "Permission";

INSERT INTO "RolePermissions" ("Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "RolePermission";

INSERT INTO "UserPermissions" ("Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "UserPermission"; 