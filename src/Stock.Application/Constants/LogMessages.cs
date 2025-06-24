namespace Stock.Application.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan log mesajlarını içeren sınıf.
    /// Bu sınıf, uygulamanın farklı bölümlerinde kullanılan log mesajlarını standart bir şekilde tanımlar.
    /// </summary>
    /// <remarks>
    /// Log mesajları kategorilere ayrılmıştır:
    /// - Genel log mesajları
    /// - Kullanıcı ile ilgili log mesajları
    /// - Rol ile ilgili log mesajları
    /// - İzin ile ilgili log mesajları
    /// - Ürün ile ilgili log mesajları
    /// - Kategori ile ilgili log mesajları
    /// </remarks>
    public static class LogMessages
    {
        #region Genel Log Mesajları

        /// <summary>
        /// Uygulama başlatılırken kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// Program.cs veya Startup.cs içinde kullanılır.
        /// </remarks>
        public const string ApplicationStarting = "Uygulama başlatılıyor...";

        /// <summary>
        /// Uygulama başarıyla başlatıldığında kaydedilen log mesajı.
        /// </summary>
        public const string ApplicationStarted = "Uygulama başlatıldı";

        /// <summary>
        /// Uygulama durdurulurken kaydedilen log mesajı.
        /// </summary>
        public const string ApplicationStopping = "Uygulama durduruluyor...";

        /// <summary>
        /// Uygulama başarıyla durdurulduğunda kaydedilen log mesajı.
        /// </summary>
        public const string ApplicationStopped = "Uygulama durduruldu";

        /// <summary>
        /// İşlem başlatıldığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi işlem adını temsil eder.
        /// </remarks>
        public const string OperationStarted = "İşlem başlatıldı: {0}";

        /// <summary>
        /// İşlem tamamlandığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi işlem adını temsil eder.
        /// </remarks>
        public const string OperationCompleted = "İşlem tamamlandı: {0}";

        /// <summary>
        /// İşlem başarısız olduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi işlem adını temsil eder.
        /// </remarks>
        public const string OperationFailed = "İşlem başarısız oldu: {0}";

        #endregion

        #region Kullanıcı ile İlgili Log Mesajları

        /// <summary>
        /// Kullanıcı giriş denemesi yapıldığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLoginAttempt = "Kullanıcı giriş denemesi: {0}";

        /// <summary>
        /// Kullanıcı başarıyla giriş yaptığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLoginSuccess = "Kullanıcı başarıyla giriş yaptı: {0}";

        /// <summary>
        /// Kullanıcı girişi başarısız olduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLoginFailed = "Kullanıcı girişi başarısız: {0}";

        /// <summary>
        /// Kullanıcı çıkış yaptığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLogout = "Kullanıcı çıkış yaptı: {0}";

        /// <summary>
        /// Yeni kullanıcı oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını, {1} parametresi sicil numarasını temsil eder.
        /// </remarks>
        public const string UserCreating = "Yeni kullanıcı oluşturma isteği alındı: {0}, Sicil: {1}";

        /// <summary>
        /// Kullanıcı başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını, {1} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserCreated = "Kullanıcı başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// Kullanıcı güncelleme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserUpdating = "Kullanıcı güncelleme isteği alındı: ID: {0}";

        /// <summary>
        /// Kullanıcı başarıyla güncellendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserUpdated = "ID: {0} olan kullanıcı güncellendi.";

        /// <summary>
        /// Kullanıcı silme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserDeleting = "ID: {0} olan kullanıcı siliniyor...";

        /// <summary>
        /// Kullanıcı başarıyla silindiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserDeleted = "ID: {0} olan kullanıcı silindi.";

        /// <summary>
        /// Şifre değiştirme isteği başlatıldığında kaydedilen log mesajı.
        /// </summary>
        public const string PasswordChanging = "Şifre değiştirme isteği başlatılıyor...";

        /// <summary>
        /// Şifre başarıyla değiştirildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string PasswordChanged = "Şifre başarıyla değiştirildi - Kullanıcı ID: {0}";

        /// <summary>
        /// Şifre değiştirme işlemi başarısız olduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi hata mesajını temsil eder.
        /// </remarks>
        public const string PasswordChangeFailed = "Şifre değiştirme başarısız - Kullanıcı ID: {0}, Hata: {1}";

        /// <summary>
        /// Yeni kullanıcı oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını, {1} parametresi sicil numarasını temsil eder.
        /// </remarks>
        public const string UserCreationRequest = "Yeni kullanıcı oluşturma isteği alındı: {0}, Sicil: {1}";

        /// <summary>
        /// Kullanıcı başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını, {1} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserCreatedSuccessfully = "Kullanıcı başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// Eksik parametre durumunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi eksik parametre adını temsil eder.
        /// </remarks>
        public const string MissingParameter = "Eksik parametre: {0} boş";

        /// <summary>
        /// Geçersiz rol ID durumunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi geçersiz rol ID'sini temsil eder.
        /// </remarks>
        public const string InvalidRoleId = "Geçersiz Rol ID: {0}";

        /// <summary>
        /// Kullanıcı oluşturma sırasında doğrulama hatası oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi doğrulama hata mesajını temsil eder.
        /// </remarks>
        public const string ValidationErrorDuringUserCreation = "Kullanıcı oluşturma işlemi sırasında doğrulama hatası: {0}";

        /// <summary>
        /// Kullanıcı oluşturma sırasında hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi hata mesajını temsil eder.
        /// </remarks>
        public const string ErrorDuringUserCreation = "Kullanıcı oluşturma sırasında bir hata meydana geldi: {0}";

        /// <summary>
        /// İç hata durumunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi iç hata mesajını temsil eder.
        /// </remarks>
        public const string InnerError = "İç hata: {0}";

        /// <summary>
        /// Token içinde kullanıcı ID bulunamadığında veya geçersiz olduğunda kaydedilen log mesajı.
        /// </summary>
        public const string UserIdNotFoundInToken = "Token'da kullanıcı ID'si bulunamadı veya geçersiz";

        /// <summary>
        /// Belirli bir kullanıcı için şifre değiştirme isteği log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string PasswordChangeRequestForUser = "Şifre değiştirme isteği - Kullanıcı ID: {0}";

        /// <summary>
        /// Şifre başarıyla değiştirildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string PasswordChangeSuccessful = "Şifre başarıyla değiştirildi - Kullanıcı ID: {0}";

        /// <summary>
        /// Tüm kullanıcılar getiriliyor...
        /// </summary>
        public const string GettingAllUsers = "Tüm kullanıcılar getiriliyor...";

        /// <summary>
        /// Toplam {0} kullanıcı getirildi.
        /// </summary>
        /// <remarks>
        /// {0} parametresi getirilen kullanıcı sayısını temsil eder.
        /// </remarks>
        public const string UsersRetrieved = "Toplam {0} kullanıcı getirildi.";

        /// <summary>
        /// Kullanıcılar getirilirken bir hata oluştu.
        /// </summary>
        public const string ErrorGettingUsers = "Kullanıcılar getirilirken bir hata oluştu.";

        /// <summary>
        /// ID: {0} olan kullanıcı getiriliyor...
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string GettingUserById = "ID: {0} olan kullanıcı getiriliyor...";

        /// <summary>
        /// ID: {0} olan kullanıcı getirildi.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserRetrieved = "ID: {0} olan kullanıcı getirildi.";

        /// <summary>
        /// ID: {0} olan kullanıcı bulunamadı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserNotFoundById = "ID: {0} olan kullanıcı bulunamadı.";

        /// <summary>
        /// ID: {0} olan kullanıcı getirilirken bir hata oluştu.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string ErrorGettingUserById = "ID: {0} olan kullanıcı getirilirken bir hata oluştu.";

        /// <summary>
        /// Yeni kullanıcı oluşturuluyor: {0}
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string CreatingUser = "Yeni kullanıcı oluşturuluyor: {0}";

        /// <summary>
        /// Bu kullanıcı adı zaten kullanılıyor: {0}
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string DuplicateUsername = "Bu kullanıcı adı zaten kullanılıyor: {0}";

        /// <summary>
        /// Bu sicil numarası zaten kullanılıyor: {0}
        /// </summary>
        /// <remarks>
        /// {0} parametresi sicil numarasını temsil eder.
        /// </remarks>
        public const string DuplicateSicil = "Bu sicil numarası zaten kullanılıyor: {0}";

        /// <summary>
        /// ID: {0} olan kullanıcı güncelleniyor...
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UpdatingUser = "ID: {0} olan kullanıcı güncelleniyor...";

        /// <summary>
        /// ID: {0} olan kullanıcı siliniyor...
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string DeletingUser = "ID: {0} olan kullanıcı siliniyor...";

        /// <summary>
        /// ID: {0} olan kullanıcı silinirken bir hata oluştu.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string ErrorDeletingUser = "ID: {0} olan kullanıcı silinirken bir hata oluştu.";

        /// <summary>
        /// ID: {0} olan admin kullanıcısı silinemez.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string CannotDeleteAdminUser = "ID: {0} olan admin kullanıcısı silinemez.";

        /// <summary>
        /// ID: {0} olan kullanıcı kendisini silemez.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string CannotDeleteSelf = "ID: {0} olan kullanıcı kendisini silemez.";

        /// <summary>
        /// ID: {0} olan kullanıcının rolü {1} olarak güncelleniyor...
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi yeni rolü temsil eder.
        /// </remarks>
        public const string UpdatingUserRole = "ID: {0} olan kullanıcının rolü {1} olarak güncelleniyor...";

        /// <summary>
        /// ID: {0} olan kullanıcının rolü {1} olarak güncellendi.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi yeni rolü temsil eder.
        /// </remarks>
        public const string UserRoleUpdated = "ID: {0} olan kullanıcının rolü {1} olarak güncellendi.";

        /// <summary>
        /// ID: {0} olan kullanıcının rolü {1} olarak güncellenirken bir hata oluştu.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi hata mesajını temsil eder.
        /// </remarks>
        public const string ErrorUpdatingUserRole = "ID: {0} olan kullanıcının rolü {1} olarak güncellenirken bir hata oluştu.";

        /// <summary>
        /// ID: {0} olan rol bulunamadı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFoundById = "ID: {0} olan rol bulunamadı.";

        /// <summary>
        /// Kullanıcı oluşturma sırasında çakışma (örn. sicil) olduğunda log mesajı.
        /// </summary>
        public const string ConflictDuringUserCreation = "Kullanıcı oluşturma sırasında çakışma: Sicil {0}, Hata: {1}";

        /// <summary>
        /// Kullanıcı oluşturma sırasında ilişkili bir kayıt (örn. rol) bulunamadığında log mesajı.
        /// </summary>
        public const string NotFoundDuringUserCreation = "Kullanıcı oluşturma sırasında ilgili kayıt bulunamadı: {0}";

        /// <summary>
        /// Kullanıcı güncelleme sırasında çakışma (örn. sicil) olduğunda log mesajı.
        /// </summary>
        public const string ConflictDuringUserUpdate = "Kullanıcı güncelleme sırasında çakışma: ID {0}, Hata: {1}";

        /// <summary>
        /// Kullanıcı güncelleme sırasında ilişkili bir kayıt (örn. rol) bulunamadığında log mesajı.
        /// </summary>
        public const string NotFoundDuringUserUpdate = "Kullanıcı güncelleme sırasında ilgili kayıt bulunamadı: ID {0}, Hata: {1}";

        /// <summary>
        /// Kullanıcı silme sırasında kullanıcı bulunamadığında log mesajı.
        /// </summary>
        public const string NotFoundDuringUserDelete = "Silinmek istenen kullanıcı bulunamadı: ID {0}";

        /// <summary>
        /// Kullanıcı silme sırasında genel hata log mesajı.
        /// </summary>
        public const string ErrorDuringUserDelete = "Kullanıcı silme sırasında hata: ID {0}";

        #endregion

        #region Rol ile İlgili Log Mesajları

        /// <summary>
        /// Yeni rol oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol adını temsil eder.
        /// </remarks>
        public const string RoleCreating = "Yeni rol oluşturma isteği alındı: {0}";

        /// <summary>
        /// Rol başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol adını, {1} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleCreated = "Rol başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// Rol güncelleme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleUpdating = "Rol güncelleme isteği alındı: ID: {0}";

        /// <summary>
        /// Rol başarıyla güncellendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleUpdated = "Rol başarıyla güncellendi: ID: {0}";

        /// <summary>
        /// Rol silme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleDeleting = "Rol silme isteği alındı: ID: {0}";

        /// <summary>
        /// Rol başarıyla silindiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleDeleted = "Rol başarıyla silindi: ID: {0}";

        /// <summary>
        /// Tüm roller getirilirken kaydedilen log mesajı.
        /// </summary>
        public const string FetchingAllRoles = "Tüm roller getiriliyor...";

        /// <summary>
        /// Roller başarıyla getirildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi getirilen rol sayısını temsil eder.
        /// </remarks>
        public const string RolesFetchedSuccessfully = "{0} rol başarıyla getirildi.";

        /// <summary>
        /// Roller getirilirken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        public const string ErrorFetchingRoles = "Roller getirilirken bir hata oluştu.";

        /// <summary>
        /// Belirli bir rol getirilirken kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string FetchingRoleById = "ID: {0} olan rol getiriliyor...";

        /// <summary>
        /// Rol bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFound = "Rol bulunamadı: ID {0}";

        /// <summary>
        /// Rol başarıyla getirildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleFetchedSuccessfully = "Rol başarıyla getirildi: ID {0}";

        /// <summary>
        /// Rol getirilirken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string ErrorFetchingRole = "Rol getirilirken hata oluştu: ID {0}";

        /// <summary>
        /// Rol oluşturulurken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        public const string ErrorCreatingRole = "Rol oluşturulurken bir hata oluştu.";

        /// <summary>
        /// Güncelleme için rol bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFoundForUpdate = "Güncellenmek istenen rol bulunamadı: ID {0}";

        /// <summary>
        /// Eşzamanlılık hatası nedeniyle rol bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFoundDuringConcurrency = "Rol güncelleme sırasında bulunamadı (eşzamanlılık hatası): ID {0}";

        /// <summary>
        /// Rol güncellenirken eşzamanlılık hatası oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string ConcurrencyErrorUpdatingRole = "Rol güncellenirken eşzamanlılık hatası oluştu: ID {0}";

        /// <summary>
        /// Rol güncellenirken genel hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string ErrorUpdatingRole = "Rol güncellenirken hata oluştu: ID {0}";

        /// <summary>
        /// Silme için rol bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFoundForDelete = "Silinmek istenen rol bulunamadı: ID {0}";

        /// <summary>
        /// Rol kullanımda olduğu için silinemediğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleInUseCannotDelete = "Rol kullanımda olduğu için silinemiyor: ID {0}";

        /// <summary>
        /// Rol silinirken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string ErrorDeletingRole = "Rol silinirken hata oluştu: ID {0}";

        #endregion

        #region İzin ile İlgili Log Mesajları

        /// <summary>
        /// Yeni izin oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin adını temsil eder.
        /// </remarks>
        public const string PermissionCreating = "Yeni izin oluşturma isteği alındı: {0}";

        /// <summary>
        /// İzin başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin adını, {1} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionCreated = "İzin başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// İzin güncelleme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionUpdating = "İzin güncelleme isteği alındı: ID: {0}";

        /// <summary>
        /// İzin başarıyla güncellendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionUpdated = "İzin başarıyla güncellendi: ID: {0}";

        /// <summary>
        /// İzin silme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionDeleting = "İzin silme isteği alındı: ID: {0}";

        /// <summary>
        /// İzin başarıyla silindiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionDeleted = "İzin başarıyla silindi: ID: {0}";

        /// <summary>
        /// Kullanıcıya izinler atanırken kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi atanan izin sayısını temsil eder.
        /// </remarks>
        public const string UserPermissionAssigning = "Kullanıcıya izinler atanıyor - Kullanıcı ID: {0}, İzin sayısı: {1}";

        /// <summary>
        /// Kullanıcıya belirli bir izin atanamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi izin ID'sini temsil eder.
        /// </remarks>
        public const string PermissionAssignFailed = "İzin atanamadı - Kullanıcı ID: {0}, İzin ID: {1}";

        /// <summary>
        /// Kullanıcıya izinler atanırken genel bir hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserPermissionAssignError = "Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {0}";

        /// <summary>
        /// Eksik izinler eklenirken kaydedilen log mesajı.
        /// </summary>
        public const string AddingMissingPermissions = "Eksik izinler ekleniyor...";

        /// <summary>
        /// Bir iznin zaten mevcut olduğunu belirten log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin adını temsil eder.
        /// </remarks>
        public const string PermissionAlreadyExists = "{0} izni zaten mevcut";

        /// <summary>
        /// Yeni bir iznin başarıyla eklendiğini belirten log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi izin adını temsil eder.
        /// </remarks>
        public const string PermissionAdded = "{0} izni eklendi";

        /// <summary>
        /// Kullanıcıya toplu izin atama işlemi tamamlandığında log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserPermissionsAssigned = "Kullanıcıya izin atama işlemi tamamlandı: UserId={0}";

        /// <summary>
        /// Eksik izinler eklenirken hata oluştuğunda log mesajı.
        /// </summary>
        public const string ErrorAddingMissingPermissions = "Eksik izinler eklenirken hata oluştu.";

        #endregion

        #region Ürün ile İlgili Log Mesajları

        /// <summary>
        /// Yeni ürün oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün adını temsil eder.
        /// </remarks>
        public const string ProductCreating = "Yeni ürün oluşturma isteği alındı: {0}";

        /// <summary>
        /// Ürün başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün adını, {1} parametresi ürün ID'sini temsil eder.
        /// </remarks>
        public const string ProductCreated = "Ürün başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// Ürün güncelleme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün ID'sini temsil eder.
        /// </remarks>
        public const string ProductUpdating = "Ürün güncelleme isteği alındı: ID: {0}";

        /// <summary>
        /// Ürün başarıyla güncellendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün ID'sini temsil eder.
        /// </remarks>
        public const string ProductUpdated = "Ürün başarıyla güncellendi: ID: {0}";

        /// <summary>
        /// Ürün silme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün ID'sini temsil eder.
        /// </remarks>
        public const string ProductDeleting = "Ürün silme isteği alındı: ID: {0}";

        /// <summary>
        /// Ürün başarıyla silindiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ürün ID'sini temsil eder.
        /// </remarks>
        public const string ProductDeleted = "Ürün başarıyla silindi: ID: {0}";

        #endregion

        #region Kategori ile İlgili Log Mesajları

        /// <summary>
        /// Yeni kategori oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori adını temsil eder.
        /// </remarks>
        public const string CategoryCreating = "Yeni kategori oluşturma isteği alındı: {0}";

        /// <summary>
        /// Kategori başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori adını, {1} parametresi kategori ID'sini temsil eder.
        /// </remarks>
        public const string CategoryCreated = "Kategori başarıyla oluşturuldu: {0}, ID: {1}";

        /// <summary>
        /// Kategori güncelleme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori ID'sini temsil eder.
        /// </remarks>
        public const string CategoryUpdating = "Kategori güncelleme isteği alındı: ID: {0}";

        /// <summary>
        /// Kategori başarıyla güncellendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori ID'sini temsil eder.
        /// </remarks>
        public const string CategoryUpdated = "Kategori başarıyla güncellendi: ID: {0}";

        /// <summary>
        /// Kategori silme isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori ID'sini temsil eder.
        /// </remarks>
        public const string CategoryDeleting = "Kategori silme isteği alındı: ID: {0}";

        /// <summary>
        /// Kategori başarıyla silindiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kategori ID'sini temsil eder.
        /// </remarks>
        public const string CategoryDeleted = "Kategori başarıyla silindi: ID: {0}";

        #endregion

        #region Oturum işlemleri

        /// <summary>
        /// Kullanıcı giriş yaptığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLoggedIn = "Kullanıcı giriş yaptı. ID: {0}, Kullanıcı Adı: {1}";

        /// <summary>
        /// Kullanıcı çıkış yaptığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string UserLoggedOut = "Kullanıcı çıkış yaptı. ID: {0}, Kullanıcı Adı: {1}";

        /// <summary>
        /// Geçersiz giriş denemesi yapıldığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string InvalidLoginAttempt = "Geçersiz giriş denemesi. Kullanıcı Adı: {0}";

        /// <summary>
        /// Hesap kilitlendiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı adını temsil eder.
        /// </remarks>
        public const string AccountLocked = "Hesap kilitlendi. Kullanıcı Adı: {0}";

        /// <summary>
        /// Şifre değiştirme isteği başladığında log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string PasswordChangeRequestStarted = "Şifre değiştirme isteği başlatıldı: Kullanıcı ID {0}";

        #endregion

        #region Performans izleme

        /// <summary>
        /// Yavaş sorgu tespit edildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi sorgunun süresini, {1} parametresi sorgu metnini temsil eder.
        /// </remarks>
        public const string SlowQuery = "Yavaş sorgu tespit edildi. Süre: {0}ms, Sorgu: {1}";

        /// <summary>
        /// Yüksek bellek kullanımı tespit edildiğinde kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanılan bellek miktarını MB cinsinden temsil eder.
        /// </remarks>
        public const string HighMemoryUsage = "Yüksek bellek kullanımı tespit edildi. Kullanılan: {0}MB";

        /// <summary>
        /// Veritabanı bağlantı sorunu olduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi hata mesajını temsil eder.
        /// </remarks>
        public const string DatabaseConnectionIssue = "Veritabanı bağlantı sorunu: {0}";

        #endregion

        #region Şifreleme Log Mesajları
        public const string PasswordCannotBeNullOrEmptyForHashing = "Hashlenecek şifre null veya boş olamaz.";
        public const string PasswordHashingStarted = "Şifre hashleme başlatıldı. Work Factor: {0}";
        public const string PasswordHashedSuccessfully = "Şifre başarıyla hashlendi: {0}";
        public const string PasswordHashingSaltError = "Şifre hashlenirken salt ayrıştırma hatası oluştu.";
        public const string PasswordHashingGenericError = "Şifre hashlenirken genel bir hata oluştu.";
        public const string PasswordOrHashCannotBeNullOrEmptyForVerification = "Doğrulanacak şifre veya hash null veya boş olamaz.";
        public const string PasswordVerificationStarted = "Şifre doğrulaması başlatıldı.";
        public const string PasswordVerificationResultLog = "Şifre doğrulama sonucu: {0}";
        public const string PasswordVerificationSaltError = "Şifre doğrulanırken salt ayrıştırma hatası oluştu.";
        public const string PasswordVerificationGenericError = "Şifre doğrulanırken genel bir hata oluştu.";
        public const string HashFormatLog = "Hash Formatı: {0}";
        public const string HashLengthLog = "Hash Uzunluğu: {0}";
        public const string WorkFactorLog = "Work Factor: {0}";
        public const string ErrorParsingWorkFactor = "Work factor ayrıştırılırken hata oluştu.";
        #endregion

        public const string ErrorCreatingUser = "Kullanıcı oluşturulurken bir hata oluştu: {0}";
        public const string ErrorUpdatingUser = "Kullanıcı güncellenirken bir hata oluştu: ID {0}, Hata: {1}";
        public const string LoginAttempt = "Giriş denemesi: Sicil {0}, Şifre Uzunluğu: {1}";
        public const string UserNotFoundBySicil = "Sicil numarası ile kullanıcı bulunamadı: {0}";
        public const string UserFound = "Kullanıcı bulundu: Sicil {0}, ID: {1}, Admin: {2}";
        public const string PasswordVerificationStarting = "Şifre doğrulaması başlatılıyor. Hash: {0}";
        public const string HashLength = "Hash uzunluğu: {0}";
        public const string HashFormat = "Hash formatı: {0}";
        public const string PasswordVerificationResult = "Şifre doğrulama sonucu: {0}";
        public const string PasswordVerificationError = "Şifre doğrulaması sırasında hata oluştu.";
        public const string InvalidPasswordAttempt = "Geçersiz şifre denemesi: Sicil {0}";
        public const string LoginSuccess = "Başarılı giriş: Sicil {0}";
        public const string RegisterAttempt = "Kayıt denemesi: Kullanıcı Adı {0}";
        public const string UsernameAlreadyExists = "Kullanıcı adı zaten mevcut: {0}";
        public const string PasswordHashed = "Şifre hash'lendi: Kullanıcı Adı {0}, Hash: {1}";
        public const string UserAdminStatusCheck = "Kullanıcı admin durumu kontrolü: Admin={0}, RoleId={1}";
        public const string UserRegistered = "Kullanıcı kaydedildi: Kullanıcı Adı {0}, ID: {1}, Admin: {2}";
        public const string TokenGenerated = "Token oluşturuldu: Kullanıcı Adı {0}";
        public const string RegistrationError = "Kayıt sırasında hata: Kullanıcı Adı {0}";
        public const string ChangePasswordAttempt = "Şifre değiştirme denemesi: Kullanıcı ID {0}";
        public const string IncorrectOldPassword = "Yanlış eski şifre: Kullanıcı ID {0}";
        public const string PasswordMismatch = "Şifreler uyuşmuyor: Kullanıcı ID {0}";
        public const string PasswordChangedSuccessfully = "Şifre başarıyla değiştirildi: Kullanıcı ID {0}";
        public const string ChangePasswordError = "Şifre değiştirme sırasında hata: Kullanıcı ID {0}";
        public const string ErrorGettingCustomUserPermissions = "Özel kullanıcı izinleri alınırken hata: Kullanıcı ID {0}";
        public const string AssigningPermissionsToRole = "Role izinler atanıyor: RoleId {0}, İzin Sayısı {1}";
        public const string PermissionsAssignedToRole = "İzinler role başarıyla atandı: RoleId {0}, İzin Sayısı {1}";
        public const string ErrorAssigningPermissionsToRole = "Role izin atanırken hata: RoleId {0}";
        public const string UserPermissionUpdated = "Kullanıcı izni güncellendi: IsGranted={0}, UserId={1}, PermissionId={2}";
        public const string UserPermissionAdded = "Kullanıcı izni eklendi: IsGranted={0}, UserId={1}, PermissionId={2}";
        public const string ErrorAssigningUserPermission = "Kullanıcıya izin atanırken hata: UserId={0}, PermissionId={1}";
        public const string UserPermissionRemoved = "Kullanıcı izni kaldırıldı: UserId={0}, PermissionId={1}";
        public const string UserPermissionNotFoundForRemoval = "Kaldırılacak kullanıcı izni bulunamadı: UserId={0}, PermissionId={1}";
        public const string ErrorRemovingUserPermission = "Kullanıcı izni kaldırılırken hata: UserId={0}, PermissionId={1}";
        public const string UserPermissionsReset = "Kullanıcı özel izinleri sıfırlandı: UserId={0}, Kaldırılan İzin Sayısı: {1}";
        public const string UserPermissionsResetNoAction = "Sıfırlanacak özel kullanıcı izni yok: UserId={0}";
        public const string ErrorResettingUserPermissions = "Kullanıcı izinleri sıfırlanırken hata: UserId={0}";
        public const string ErrorCheckingUserPermission = "Kullanıcı izni kontrol edilirken hata: UserId={0}, PermissionName={1}";
        public const string ErrorGettingAllPermissions = "Tüm izinler alınırken hata oluştu.";
        public const string ErrorGettingPermissionsByGroup = "İzinler gruplara göre alınırken hata oluştu.";
        public const string ErrorGettingPermissionsByRole = "Role göre izinler alınırken hata: RoleId={0}";
        public const string ErrorGettingPermissionsByUser = "Kullanıcıya göre izinler alınırken hata: UserId={0}";

        /// <summary>
        /// Kullanıcıya izin atanırken log mesajı.
        /// </summary>
        public const string AssigningPermissionToUser = "Kullanıcıya izin atanıyor/güncelleniyor: UserId={0}, PermissionId={1}, IsGranted={2}";

        /// <summary>
        /// Kullanıcıya izin başarıyla atandığında log mesajı.
        /// </summary>
        public const string PermissionAssignedToUser = "Kullanıcıya izin başarıyla atandı/güncellendi: UserId={0}, PermissionId={1}, IsGranted={2}";

        /// <summary>
        /// Kullanıcıdan izin kaldırılırken log mesajı.
        /// </summary>
        public const string RemovingPermissionFromUser = "Kullanıcıdan izin kaldırılıyor: UserId={0}, PermissionId={1}";

        /// <summary>
        /// Kullanıcıdan izin başarıyla kaldırıldığında log mesajı.
        /// </summary>
        public const string PermissionRemovedFromUser = "Kullanıcıdan izin başarıyla kaldırıldı: UserId={0}, PermissionId={1}";

        /// <summary>
        /// Kullanıcının izinleri rol izinlerine sıfırlanırken log mesajı.
        /// </summary>
        public const string ResettingPermissionsForUser = "Kullanıcının özel izinleri sıfırlanıyor: UserId={0}";

        /// <summary>
        /// Kullanıcının izinleri başarıyla rol izinlerine sıfırlandığında log mesajı.
        /// </summary>
        public const string PermissionsResetForUser = "Kullanıcının özel izinleri başarıyla sıfırlandı: UserId={0}";

        /// <summary>
        /// Tüm kullanıcılar getirilirken kaydedilen log mesajı.
        /// </summary>
        public const string FetchingAllUsers = "Tüm kullanıcılar getiriliyor...";

        /// <summary>
        /// Kullanıcıların başarıyla getirildiğini belirten log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi getirilen kullanıcı sayısını temsil eder.
        /// </remarks>
        public const string UsersFetchedSuccessfully = "{0} kullanıcı başarıyla getirildi.";

        /// <summary>
        /// Kullanıcılar getirilirken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        public const string ErrorFetchingUsers = "Kullanıcılar getirilirken bir hata oluştu.";

        /// <summary>
        /// Belirli bir ID'ye sahip kullanıcı getirilirken kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string FetchingUserById = "ID: {0} olan kullanıcı getiriliyor...";

        /// <summary>
        /// Kullanıcı bulunamadığında kaydedilen log mesajı (Genel).
        /// </summary>
        public const string UserNotFound = "Kullanıcı bulunamadı.";

        /// <summary>
        /// Kullanıcının başarıyla getirildiğini belirten log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserFetchedSuccessfully = "ID: {0} olan kullanıcı başarıyla getirildi.";

        /// <summary>
        /// Belirli bir kullanıcı getirilirken hata oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string ErrorFetchingUser = "ID: {0} olan kullanıcı getirilirken bir hata oluştu.";

        /// <summary>
        /// Route parametresi ile body ID'si eşleşmediğinde log mesajı.
        /// </summary>
         /// <remarks>
        /// {0} Route parametresini, {1} body ID'sini temsil eder.
        /// </remarks>
        public const string RouteBodyIdMismatch = "Route parametresi ({0}) ile istek gövdesindeki ID ({1}) eşleşmiyor.";

        /// <summary>
        /// Güncelleme için kullanıcı bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserNotFoundForUpdate = "Güncellenmek istenen kullanıcı bulunamadı: ID {0}";

        /// <summary>
        /// Kullanıcı güncelleme sırasında doğrulama hatası oluştuğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} doğrulama hatalarını temsil eder.
        /// </remarks>
        public const string ValidationErrorDuringUserUpdate = "Kullanıcı güncelleme işlemi sırasında doğrulama hatası: ID {0}, Hatalar: {1}";

        /// <summary>
        /// Silme için kullanıcı bulunamadığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserNotFoundForDelete = "Silinmek istenen kullanıcı bulunamadı: ID {0}";

        public const string InvalidCredentials = "Geçersiz kullanıcı adı veya şifre";

        /// <summary>
        /// Kullanıcı adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        public const string UserAlreadyExists = "Bu kullanıcı adı zaten kullanılmaktadır";

        /// <summary>
        /// Sicil numarasının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        public const string SicilAlreadyExists = "Bu sicil numarası zaten kullanılmaktadır";

        /// <summary>
        /// Sicil numarası çakışması için AuthService içinde kullanılacak log mesajı.
        /// </summary>
        public const string LogSicilAlreadyExists = "Sicil numarası zaten kayıtlı: {0}";

        /// <summary>
        /// Sicil numarası ile kullanıcı bulunamadığında AuthService içinde kullanılacak log mesajı.
        /// </summary>
        public const string LogUserNotFoundBySicil = "Sicil numarası ile kullanıcı bulunamadı: {0}";

        /// <summary>
        /// Geçersiz şifre denemesi için AuthService içinde kullanılacak log mesajı.
        /// </summary>
        public const string LogInvalidPasswordAttempt = "Geçersiz şifre denemesi: Sicil {0}";

        /// <summary>
        /// Başarılı giriş için AuthService içinde kullanılacak log mesajı.
        /// </summary>
        public const string LogLoginSuccess = "Başarılı giriş: Sicil {0}";

        /// <summary>
        /// Kayıt denemesi için AuthService içinde kullanılacak log mesajı.
        /// </summary>
        public const string LogRegisterAttempt = "Kayıt denemesi: Sicil {0}";

        /// <summary>
        /// Şifre hashleme logu (Sicil ile) - AuthService içinde kullanılacak.
        /// </summary>
        public const string LogPasswordHashedForSicil = "Şifre hash'lendi: Sicil {0}, Hash: {1}";

        /// <summary>
        /// Kullanıcı kaydedildi logu (Sicil ile) - AuthService içinde kullanılacak.
        /// </summary>
        public const string LogUserRegisteredWithSicil = "Kullanıcı kaydedildi: Sicil {0}, ID: {1}, Admin: {2}";

        /// <summary>
        /// Token oluşturuldu logu (Sicil ile) - AuthService içinde kullanılacak.
        /// </summary>
        public const string LogTokenGeneratedForSicil = "Token oluşturuldu: Sicil {0}";

        /// <summary>
        /// Kayıt hatası logu (Sicil ile) - AuthService içinde kullanılacak.
        /// </summary>
        public const string LogRegistrationErrorWithSicil = "Kayıt sırasında hata: Sicil {0}";

        /// <summary>
        /// Kullanıcı oluşturma isteği alındığında kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi Sicil numarasını temsil eder.
        /// </remarks>
        public const string CreatingUserWithSicil = "Yeni kullanıcı oluşturuluyor: Sicil {0}";

        /// <summary>
        /// Kullanıcı başarıyla oluşturulduğunda kaydedilen log mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini, {1} parametresi Sicil numarasını temsil eder.
        /// </remarks>
        public const string UserCreatedWithSicil = "Kullanıcı başarıyla oluşturuldu: ID: {0}, Sicil: {1}";

        /// <summary>
        /// Kullanıcı oluşturma sırasında hata oluştuğunda kaydedilen log mesajı (Sicil ile).
        /// </summary>
        public const string ErrorCreatingUserWithSicil = "Kullanıcı oluşturma sırasında bir hata meydana geldi: Sicil {0}";

        // DDD-related error messages
        public const string InvalidUserData = "Geçersiz kullanıcı verisi: {0}";
    }
} 