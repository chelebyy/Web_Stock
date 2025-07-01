-- Permissions tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "Permissions_Backup" (
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

-- Verileri yedek tabloya kopyala
INSERT INTO "Permissions_Backup" 
SELECT * FROM "Permissions";

-- RolePermissions tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "RolePermissions_Backup" (
    "Id" integer PRIMARY KEY,
    "RoleId" integer NOT NULL,
    "PermissionId" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "IsDeleted" boolean NOT NULL
);

-- Verileri yedek tabloya kopyala
INSERT INTO "RolePermissions_Backup" 
SELECT * FROM "RolePermissions";

-- UserPermissions tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "UserPermissions_Backup" (
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

-- Verileri yedek tabloya kopyala
INSERT INTO "UserPermissions_Backup" 
SELECT * FROM "UserPermissions"; 