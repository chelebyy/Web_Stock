-- Önce izinlerin var olup olmadığını kontrol et
IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'IT.Access')
BEGIN
    -- IT.Access iznini ekle
    INSERT INTO Permissions (Name, Description, ResourceType, [Group], CreatedAt)
    VALUES ('IT.Access', 'Bilgi İşlem modülüne erişim izni', 'Page', 'Pages', GETUTCDATE());
    
    PRINT 'IT.Access izni oluşturuldu.';
END
ELSE
BEGIN
    PRINT 'IT.Access izni zaten mevcut.';
END

-- Diğer gerekli izinleri ekle
IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Pages.UserDashboard')
BEGIN
    INSERT INTO Permissions (Name, Description, ResourceType, [Group], CreatedAt)
    VALUES ('Pages.UserDashboard', 'Kullanıcı paneline erişim izni', 'Page', 'Pages', GETUTCDATE());
    
    PRINT 'Pages.UserDashboard izni oluşturuldu.';
END

IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Users.Permissions.Manage')
BEGIN
    INSERT INTO Permissions (Name, Description, ResourceType, [Group], CreatedAt)
    VALUES ('Users.Permissions.Manage', 'Kullanıcı izinlerini yönetme izni', 'Function', 'Users', GETUTCDATE());
    
    PRINT 'Users.Permissions.Manage izni oluşturuldu.';
END

IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Roles.Update')
BEGIN
    INSERT INTO Permissions (Name, Description, ResourceType, [Group], CreatedAt)
    VALUES ('Roles.Update', 'Rolleri güncelleme izni', 'Function', 'Roles', GETUTCDATE());
    
    PRINT 'Roles.Update izni oluşturuldu.';
END

-- Test kullanıcısına IT.Access iznini ata
DECLARE @TestUserId INT;
DECLARE @ITAccessPermissionId INT;

-- Test kullanıcısının ID'sini bul
SELECT @TestUserId = Id FROM Users WHERE Username = 'Test';

-- IT.Access izninin ID'sini bul
SELECT @ITAccessPermissionId = Id FROM Permissions WHERE Name = 'IT.Access';

-- Eğer Test kullanıcısı ve IT.Access izni varsa, izni ata
IF @TestUserId IS NOT NULL AND @ITAccessPermissionId IS NOT NULL
BEGIN
    -- Önce bu izin zaten atanmış mı kontrol et
    IF NOT EXISTS (SELECT 1 FROM UserPermissions WHERE UserId = @TestUserId AND PermissionId = @ITAccessPermissionId)
    BEGIN
        INSERT INTO UserPermissions (UserId, PermissionId, IsGranted, CreatedAt)
        VALUES (@TestUserId, @ITAccessPermissionId, 1, GETUTCDATE());
        
        PRINT 'Test kullanıcısına IT.Access izni atandı.';
    END
    ELSE
    BEGIN
        PRINT 'Test kullanıcısına IT.Access izni zaten atanmış.';
    END
END
ELSE
BEGIN
    PRINT 'Test kullanıcısı veya IT.Access izni bulunamadı.';
END

-- Test2 kullanıcısının IT.Access iznine sahip olmadığından emin ol
DECLARE @Test2UserId INT;

-- Test2 kullanıcısının ID'sini bul
SELECT @Test2UserId = Id FROM Users WHERE Username = 'Test2';

-- Eğer Test2 kullanıcısı ve IT.Access izni varsa, izni kaldır
IF @Test2UserId IS NOT NULL AND @ITAccessPermissionId IS NOT NULL
BEGIN
    -- Bu izin atanmış mı kontrol et
    IF EXISTS (SELECT 1 FROM UserPermissions WHERE UserId = @Test2UserId AND PermissionId = @ITAccessPermissionId)
    BEGIN
        DELETE FROM UserPermissions 
        WHERE UserId = @Test2UserId AND PermissionId = @ITAccessPermissionId;
        
        PRINT 'Test2 kullanıcısından IT.Access izni kaldırıldı.';
    END
    ELSE
    BEGIN
        PRINT 'Test2 kullanıcısına IT.Access izni zaten atanmamış.';
    END
END
ELSE
BEGIN
    PRINT 'Test2 kullanıcısı veya IT.Access izni bulunamadı.';
END 