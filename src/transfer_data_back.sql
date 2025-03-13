-- Önce eski tabloları temizleyelim
TRUNCATE TABLE "Permission" CASCADE;
TRUNCATE TABLE "RolePermission" CASCADE;
TRUNCATE TABLE "UserPermission" CASCADE;

-- Verileri yeni tablolardan eski tablolara taşıyalım
INSERT INTO "Permission" ("Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "Permissions";

INSERT INTO "RolePermission" ("Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "RolePermissions";

INSERT INTO "UserPermission" ("Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "UserPermissions"; 