namespace Stock.API.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan log mesajlarını içeren sınıf
    /// </summary>
    public static class LogMessages
    {
        // Genel log mesajları
        public const string ApplicationStarting = "Uygulama başlatılıyor...";
        public const string ApplicationStarted = "Uygulama başlatıldı";
        public const string ApplicationStopping = "Uygulama durduruluyor...";
        public const string ApplicationStopped = "Uygulama durduruldu";
        
        // Kullanıcı ile ilgili log mesajları
        public const string UserLoginAttempt = "Kullanıcı giriş denemesi: {0}";
        public const string UserLoginSuccess = "Kullanıcı başarıyla giriş yaptı: {0}";
        public const string UserLoginFailed = "Kullanıcı girişi başarısız: {0}";
        public const string UserLogout = "Kullanıcı çıkış yaptı: {0}";
        public const string UserCreating = "Yeni kullanıcı oluşturma isteği alındı: {0}, Sicil: {1}";
        public const string UserCreated = "Kullanıcı başarıyla oluşturuldu: {0}, ID: {1}";
        public const string UserUpdating = "Kullanıcı güncelleme isteği alındı: ID: {0}";
        public const string UserUpdated = "Kullanıcı başarıyla güncellendi: ID: {0}";
        public const string UserDeleting = "Kullanıcı silme isteği alındı: ID: {0}";
        public const string UserDeleted = "Kullanıcı başarıyla silindi: ID: {0}";
        public const string PasswordChanging = "Şifre değiştirme isteği başlatılıyor...";
        public const string PasswordChanged = "Şifre başarıyla değiştirildi - Kullanıcı ID: {0}";
        public const string PasswordChangeFailed = "Şifre değiştirme başarısız - Kullanıcı ID: {0}, Hata: {1}";
        public const string UserCreationRequest = "Yeni kullanıcı oluşturma isteği alındı: {0}, Sicil: {1}";
        public const string UserCreatedSuccessfully = "Kullanıcı başarıyla oluşturuldu: {0}, ID: {1}";
        public const string MissingParameter = "Eksik parametre: {0} boş";
        public const string InvalidRoleId = "Geçersiz Rol ID: {0}";
        public const string ValidationErrorDuringUserCreation = "Kullanıcı oluşturma işlemi sırasında doğrulama hatası: {0}";
        public const string ErrorDuringUserCreation = "Kullanıcı oluşturma sırasında bir hata meydana geldi: {0}";
        public const string InnerError = "İç hata: {0}";
        public const string PasswordChangeRequestStarted = "Şifre değiştirme isteği başlatılıyor...";
        public const string UserIdNotFoundInToken = "Token'da kullanıcı ID'si bulunamadı veya geçersiz";
        public const string PasswordChangeRequestForUser = "Şifre değiştirme isteği - Kullanıcı ID: {0}";
        public const string PasswordChangeSuccessful = "Şifre başarıyla değiştirildi - Kullanıcı ID: {0}";
        
        // Rol ile ilgili log mesajları
        public const string RoleCreating = "Yeni rol oluşturma isteği alındı: {0}";
        public const string RoleCreated = "Rol başarıyla oluşturuldu: {0}, ID: {1}";
        public const string RoleUpdating = "Rol güncelleme isteği alındı: ID: {0}";
        public const string RoleUpdated = "Rol başarıyla güncellendi: ID: {0}";
        public const string RoleDeleting = "Rol silme isteği alındı: ID: {0}";
        public const string RoleDeleted = "Rol başarıyla silindi: ID: {0}";
        
        // İzin ile ilgili log mesajları
        public const string PermissionCreating = "Yeni izin oluşturma isteği alındı: {0}";
        public const string PermissionCreated = "İzin başarıyla oluşturuldu: {0}, ID: {1}";
        public const string PermissionUpdating = "İzin güncelleme isteği alındı: ID: {0}";
        public const string PermissionUpdated = "İzin başarıyla güncellendi: ID: {0}";
        public const string PermissionDeleting = "İzin silme isteği alındı: ID: {0}";
        public const string PermissionDeleted = "İzin başarıyla silindi: ID: {0}";
        public const string UserPermissionAssigning = "Kullanıcıya izinler atanıyor - Kullanıcı ID: {0}, İzin sayısı: {1}";
        public const string PermissionAssignFailed = "İzin atanamadı - Kullanıcı ID: {0}, İzin ID: {1}";
        public const string UserPermissionAssignError = "Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {0}";
        public const string AddingMissingPermissions = "Eksik izinler ekleniyor...";
        public const string PermissionAlreadyExists = "{0} izni zaten mevcut";
        public const string PermissionAdded = "{0} izni eklendi";
        
        // Ürün ile ilgili log mesajları
        public const string ProductCreating = "Yeni ürün oluşturma isteği alındı: {0}";
        public const string ProductCreated = "Ürün başarıyla oluşturuldu: {0}, ID: {1}";
        public const string ProductUpdating = "Ürün güncelleme isteği alındı: ID: {0}";
        public const string ProductUpdated = "Ürün başarıyla güncellendi: ID: {0}";
        public const string ProductDeleting = "Ürün silme isteği alındı: ID: {0}";
        public const string ProductDeleted = "Ürün başarıyla silindi: ID: {0}";
        
        // Kategori ile ilgili log mesajları
        public const string CategoryCreating = "Yeni kategori oluşturma isteği alındı: {0}";
        public const string CategoryCreated = "Kategori başarıyla oluşturuldu: {0}, ID: {1}";
        public const string CategoryUpdating = "Kategori güncelleme isteği alındı: ID: {0}";
        public const string CategoryUpdated = "Kategori başarıyla güncellendi: ID: {0}";
        public const string CategoryDeleting = "Kategori silme isteği alındı: ID: {0}";
        public const string CategoryDeleted = "Kategori başarıyla silindi: ID: {0}";
    }
} 