-- Kullanılmayan sayfa izinlerini silme scripti

-- Reports sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Reports');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Reports');
DELETE FROM "Permissions" WHERE "Name" = 'Pages.Reports';

-- Settings sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Settings');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Settings');
DELETE FROM "Permissions" WHERE "Name" = 'Pages.Settings';

-- StockManagement sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.StockManagement');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.StockManagement');
DELETE FROM "Permissions" WHERE "Name" = 'Pages.StockManagement';

-- Stok Yönetimi grubundaki tüm izinler
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Group" = 'Stok Yönetimi');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Group" = 'Stok Yönetimi');
DELETE FROM "Permissions" WHERE "Group" = 'Stok Yönetimi';

-- Revir sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.Revir%');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.Revir%');
DELETE FROM "Permissions" WHERE "Name" LIKE 'Pages.Revir%';

-- BilgiIslem sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.BilgiIslem%');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.BilgiIslem%');
DELETE FROM "Permissions" WHERE "Name" LIKE 'Pages.BilgiIslem%';

-- Egitim sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.Egitim%');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" LIKE 'Pages.Egitim%');
DELETE FROM "Permissions" WHERE "Name" LIKE 'Pages.Egitim%'; 