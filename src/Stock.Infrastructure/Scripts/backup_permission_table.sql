-- Permission tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "Permission_Backup" (
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
INSERT INTO "Permission_Backup" 
SELECT * FROM "Permission";

-- RolePermission tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "RolePermission_Backup" (
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
INSERT INTO "RolePermission_Backup" 
SELECT * FROM "RolePermission";

-- UserPermission tablosunun yedeğini al
CREATE TABLE IF NOT EXISTS "UserPermission_Backup" (
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
INSERT INTO "UserPermission_Backup" 
SELECT * FROM "UserPermission"; 