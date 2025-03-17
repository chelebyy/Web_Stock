namespace Stock.API.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan hata mesajlarını içeren sınıf
    /// </summary>
    public static class ErrorMessages
    {
        // Genel hata mesajları
        public const string InternalServerError = "Sunucu hatası oluştu";
        public const string ValidationError = "Doğrulama hatası oluştu";
        public const string NotFound = "Kayıt bulunamadı";
        public const string Unauthorized = "Bu işlem için yetkiniz bulunmamaktadır";
        public const string BadRequest = "Geçersiz istek";
        
        // Kullanıcı ile ilgili hata mesajları
        public const string UserNotFound = "Kullanıcı bulunamadı";
        public const string InvalidCredentials = "Geçersiz kullanıcı adı veya şifre";
        public const string UserAlreadyExists = "Bu kullanıcı adı zaten kullanılmaktadır";
        public const string SicilAlreadyExists = "Bu sicil numarası zaten kullanılmaktadır";
        public const string PasswordMismatch = "Şifreler eşleşmiyor";
        public const string OldPasswordIncorrect = "Mevcut şifre yanlış";
        public const string UserCreateError = "Kullanıcı oluşturulurken bir hata oluştu: {0}";
        public const string UserUpdateError = "Kullanıcı güncellenirken bir hata oluştu: {0}";
        public const string UserDeleteError = "Kullanıcı silinirken bir hata oluştu: {0}";
        public const string UsernameEmpty = "Kullanıcı adı boş olamaz";
        public const string PasswordEmpty = "Şifre boş olamaz";
        public const string SicilEmpty = "Sicil numarası boş olamaz";
        public const string ValidRoleRequired = "Geçerli bir rol seçilmelidir";
        public const string SicilAlreadyInUsePartial = "sicil numarası zaten kullanılmaktadır";
        public const string SicilConflict = "Sicil numarası çakışması: {0}";
        public const string UserCreationError = "Kullanıcı oluşturulurken bir hata oluştu";
        public const string InvalidUserId = "Geçersiz kullanıcı kimliği";
        
        // Rol ile ilgili hata mesajları
        public const string RoleNotFound = "Rol bulunamadı";
        public const string RoleAlreadyExists = "Bu rol adı zaten kullanılmaktadır";
        public const string RoleCreateError = "Rol oluşturulurken bir hata oluştu: {0}";
        public const string RoleUpdateError = "Rol güncellenirken bir hata oluştu: {0}";
        public const string RoleDeleteError = "Rol silinirken bir hata oluştu: {0}";
        
        // İzin ile ilgili hata mesajları
        public const string PermissionNotFound = "İzin bulunamadı";
        public const string PermissionAlreadyExists = "Bu izin adı zaten kullanılmaktadır";
        public const string PermissionCreateError = "İzin oluşturulurken bir hata oluştu: {0}";
        public const string PermissionUpdateError = "İzin güncellenirken bir hata oluştu: {0}";
        public const string PermissionDeleteError = "İzin silinirken bir hata oluştu: {0}";
        public const string UserPermissionAssignError = "Kullanıcıya izinler atanırken bir hata oluştu: {0}";
        public const string MissingPermissionsError = "Eksik izinler eklenirken bir hata oluştu: {0}";
        
        // Ürün ile ilgili hata mesajları
        public const string ProductNotFound = "Ürün bulunamadı";
        public const string ProductCreateError = "Ürün oluşturulurken bir hata oluştu: {0}";
        public const string ProductUpdateError = "Ürün güncellenirken bir hata oluştu: {0}";
        public const string ProductDeleteError = "Ürün silinirken bir hata oluştu: {0}";
        
        // Kategori ile ilgili hata mesajları
        public const string CategoryNotFound = "Kategori bulunamadı";
        public const string CategoryCreateError = "Kategori oluşturulurken bir hata oluştu: {0}";
        public const string CategoryUpdateError = "Kategori güncellenirken bir hata oluştu: {0}";
        public const string CategoryDeleteError = "Kategori silinirken bir hata oluştu: {0}";
    }
} 