-- 1. Önce yeni tabloları oluşturalım (eğer yoksa)
CREATE TABLE IF NOT EXISTS "Permissions" (
    "Id" integer PRIMARY KEY,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(200) NOT NULL,
    "ResourceType" character varying(50) NOT NULL,
    "Group" character varying(50) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "IsDeleted" boolean NOT NULL
);

CREATE TABLE IF NOT EXISTS "RolePermissions" (
    "Id" integer PRIMARY KEY,
    "RoleId" integer NOT NULL,
    "PermissionId" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "IsDeleted" boolean NOT NULL
);

CREATE TABLE IF NOT EXISTS "UserPermissions" (
    "Id" integer PRIMARY KEY,
    "UserId" integer NOT NULL,
    "PermissionId" integer NOT NULL,
    "IsGranted" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "IsDeleted" boolean NOT NULL
);

-- 2. Verileri eski tablolardan yeni tablolara taşıyalım
-- Önce mevcut verileri temizleyelim
TRUNCATE TABLE "Permissions" CASCADE;
TRUNCATE TABLE "RolePermissions" CASCADE;
TRUNCATE TABLE "UserPermissions" CASCADE;

-- Şimdi verileri taşıyalım
INSERT INTO "Permissions" ("Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "Name", "Description", "ResourceType", "Group", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "Permission";

INSERT INTO "RolePermissions" ("Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "RoleId", "PermissionId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "RolePermission";

INSERT INTO "UserPermissions" ("Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted")
SELECT "Id", "UserId", "PermissionId", "IsGranted", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsDeleted"
FROM "UserPermission";

-- 3. Yeni tablolara indeksler ve kısıtlamalar ekleyelim
-- Önce kısıtlamanın var olup olmadığını kontrol edelim
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'Permissions_Name_Unique'
    ) THEN
        ALTER TABLE "Permissions" ADD CONSTRAINT "Permissions_Name_Unique" UNIQUE ("Name");
    END IF;
END $$;

-- 4. Veri taşıma işlemi tamamlandıktan sonra eski tabloları kaldırmayı düşünebiliriz,
-- ancak önce yeni tabloların doğru çalıştığından emin olmalıyız.
-- DROP TABLE IF EXISTS "Permission" CASCADE;
-- DROP TABLE IF EXISTS "RolePermission" CASCADE;
-- DROP TABLE IF EXISTS "UserPermission" CASCADE; 