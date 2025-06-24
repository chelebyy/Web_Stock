namespace Stock.Application.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan hata mesajlarını içeren sınıf.
    /// Bu sınıf, API'nin tüm katmanlarında kullanılan hata mesajlarını standart bir şekilde tanımlar.
    /// </summary>
    /// <remarks>
    /// Hata mesajları kategorilere ayrılmıştır:
    /// - Genel hata mesajları
    /// - Kullanıcı ile ilgili hata mesajları
    /// - Rol ile ilgili hata mesajları
    /// - İzin ile ilgili hata mesajları
    /// - Ürün ile ilgili hata mesajları
    /// - Kategori ile ilgili hata mesajları
    /// </remarks>
    public static class ErrorMessages
    {
        #region Genel Hata Mesajları

        /// <summary>
        /// Sunucu tarafında beklenmeyen bir hata oluştuğunda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 500 Internal Server Error yanıtlarında kullanılır.
        /// {0} parametresi opsiyonel olarak hata detayını içerebilir.
        /// </remarks>
        public const string GeneralServerError = "Beklenmeyen bir sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin veya yönetici ile iletişime geçin. Hata: {0}";

        /// <summary>
        /// Giriş verilerinin doğrulama kurallarını karşılamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 400 Bad Request yanıtlarında kullanılır.
        /// </remarks>
        public const string ValidationError = "Doğrulama hatası oluştu";

        /// <summary>
        /// İstenen kaynağın bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 404 Not Found yanıtlarında kullanılır.
        /// </remarks>
        public const string NotFound = "Kayıt bulunamadı";

        /// <summary>
        /// Kullanıcının yetkisiz olduğu durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 401 Unauthorized veya 403 Forbidden yanıtlarında kullanılır.
        /// </remarks>
        public const string Unauthorized = "Bu işlem için yetkiniz bulunmamaktadır";

        /// <summary>
        /// Geçersiz istek formatı veya parametreleri için kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 400 Bad Request yanıtlarında kullanılır.
        /// </remarks>
        public const string BadRequest = "Geçersiz istek";

        /// <summary>
        /// Geçersiz işlem için kullanılan mesaj.
        /// </summary>
        public const string InvalidOperation = "Geçersiz işlem.";

        /// <summary>
        /// Veritabanı hatası için kullanılan mesaj.
        /// </summary>
        public const string DatabaseError = "Veritabanı hatası oluştu.";

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcı bulunamadığında kullanılan spesifik hata mesajı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi kullanıcı ID'sini temsil eder.
        /// </remarks>
        public const string UserNotFoundSpecific = "ID: {0} olan kullanıcı bulunamadı.";

        /// <summary>
        /// Route parametresi ile request body'deki ID uyuşmadığında kullanılan hata mesajı.
        /// </summary>
        public const string RouteBodyIdMismatch = "İstek URL'indeki ID ile gönderilen veri ID'si uyuşmuyor.";

        #endregion

        #region Kullanıcı ile İlgili Hata Mesajları

        /// <summary>
        /// Belirtilen kullanıcının sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string UserNotFound = "Kullanıcı bulunamadı";

        /// <summary>
        /// Kullanıcı girişi sırasında hatalı kimlik bilgileri girildiğinde kullanılan mesaj.
        /// </summary>
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
        /// Şifre ve şifre tekrarı eşleşmediğinde kullanılan mesaj.
        /// </summary>
        public const string PasswordMismatch = "Şifreler eşleşmiyor";

        /// <summary>
        /// Şifre değiştirme işleminde mevcut şifre yanlış girildiğinde kullanılan mesaj.
        /// </summary>
        public const string OldPasswordIncorrect = "Mevcut şifre yanlış";

        /// <summary>
        /// Kullanıcı oluşturma işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string UserCreateError = "Kullanıcı oluşturulurken bir hata oluştu: {0}";

        /// <summary>
        /// Kullanıcı güncelleme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string UserUpdateError = "Kullanıcı güncellenirken bir hata oluştu: {0}";

        /// <summary>
        /// Kullanıcı silme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string UserDeleteError = "Kullanıcı silinirken bir hata oluştu: {0}";

        /// <summary>
        /// Kullanıcı adı boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string UsernameEmpty = "Kullanıcı adı boş olamaz";

        /// <summary>
        /// Şifre boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string PasswordEmpty = "Şifre boş olamaz";

        /// <summary>
        /// Sicil numarası boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string SicilEmpty = "Sicil numarası boş olamaz";

        /// <summary>
        /// Ad Soyad alanı boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string AdiSoyadiEmpty = "Ad Soyad boş olamaz";

        /// <summary>
        /// Geçerli bir rol seçilmediğinde kullanılan doğrulama mesajı.
        /// </summary>
        public const string ValidRoleRequired = "Geçerli bir rol seçilmelidir";

        /// <summary>
        /// Sicil numarası çakışması kontrolü için kullanılan kısmi mesaj.
        /// </summary>
        /// <remarks>
        /// Bu mesaj, hata mesajlarında sicil çakışması kontrolü için kullanılır.
        /// </remarks>
        public const string SicilAlreadyInUsePartial = "sicil numarası zaten kullanılmaktadır";

        /// <summary>
        /// Sicil numarası çakışması durumunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string SicilConflict = "Sicil numarası çakışması: {0}";

        /// <summary>
        /// Kullanıcı oluşturma işlemi sırasında genel hata durumunda kullanılan mesaj.
        /// </summary>
        public const string UserCreationError = "Kullanıcı oluşturulurken bir hata oluştu";

        /// <summary>
        /// Geçersiz kullanıcı kimliği durumunda kullanılan mesaj.
        /// </summary>
        public const string InvalidUserId = "Geçersiz kullanıcı kimliği";

        /// <summary>
        /// Belirtilen kullanıcının sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string UserNotFoundById = "ID: {0} olan kullanıcı bulunamadı.";

        /// <summary>
        /// Kullanıcı adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string DuplicateUsername = "Bu kullanıcı adı zaten kullanılıyor: {0}";

        /// <summary>
        /// Sicil numarasının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string DuplicateSicil = "Bu sicil numarası zaten kullanılıyor: {0}";

        /// <summary>
        /// Kullanıcı kendisini silemez.
        /// </summary>
        /// <remarks>
        /// Kullanıcı silme işlemi sırasında kullanılır.
        /// </remarks>
        public const string CannotDeleteSelf = "Kullanıcı kendisini silemez.";

        /// <summary>
        /// Kullanıcı güncellenemedi.
        /// </summary>
        /// <remarks>
        /// Kullanıcı güncelleme işlemi sırasında kullanılır.
        /// </remarks>
        public const string UserUpdateFailed = "Kullanıcı güncellenemedi.";

        /// <summary>
        /// Kullanıcı silinemedi.
        /// </summary>
        /// <remarks>
        /// Kullanıcı silme işlemi sırasında kullanılır.
        /// </remarks>
        public const string UserDeleteFailed = "Kullanıcı silinemedi.";

        /// <summary>
        /// Kullanıcı oluşturulamadı.
        /// </summary>
        /// <remarks>
        /// Kullanıcı oluşturma işlemi sırasında kullanılır.
        /// </remarks>
        public const string UserCreateFailed = "Kullanıcı oluşturulamadı.";

        /// <summary>
        /// Admin kullanıcısının silinemeyeceği durumlarda kullanılan mesaj.
        /// </summary>
        public const string CannotDeleteAdminUser = "Admin kullanıcısı silinemez";

        /// <summary>
        /// Ad alanı boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string NameEmpty = "Ad boş olamaz";

        /// <summary>
        /// Soyad alanı boş bırakıldığında kullanılan doğrulama mesajı.
        /// </summary>
        public const string SurnameEmpty = "Soyad boş olamaz";

        #endregion

        #region Rol ile İlgili Hata Mesajları

        /// <summary>
        /// Belirtilen rolün sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        public const string RoleNotFound = "Rol bulunamadı";

        /// <summary>
        /// Belirtilen ID'ye sahip rol bulunamadığında kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// {0} parametresi rol ID'sini temsil eder.
        /// </remarks>
        public const string RoleNotFoundSpecific = "ID: {0} olan rol bulunamadı.";

        /// <summary>
        /// Rol oluşturma işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string RoleCreateError = "Rol oluşturulurken bir hata oluştu: {0}";

        /// <summary>
        /// Rol güncelleme işlemi sırasında eşzamanlılık hatası oluştuğunda kullanılan mesaj.
        /// </summary>
        public const string ConcurrencyError = "Veri güncellenirken çakışma oluştu. Lütfen sayfayı yenileyip tekrar deneyin.";

        /// <summary>
        /// Rol güncelleme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string RoleUpdateError = "Rol güncellenirken bir hata oluştu: {0}";

        /// <summary>
        /// Rol kullanımda olduğu için silinemediğinde kullanılan mesaj.
        /// </summary>
        public const string RoleInUseCannotDelete = "Bu rol bir veya daha fazla kullanıcı tarafından kullanıldığı için silinemez.";

        /// <summary>
        /// Rol silme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string RoleDeleteError = "Rol silinirken bir hata oluştu: {0}";

        /// <summary>
        /// Belirtilen rolün sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string RoleNotFoundById = "ID: {0} olan rol bulunamadı.";

        /// <summary>
        /// Rol adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string DuplicateRoleName = "Bu rol adı zaten kullanılıyor: {0}";

        /// <summary>
        /// Geçersiz rol ID'si için kullanılan mesaj.
        /// </summary>
        public const string InvalidRoleId = "Geçersiz rol ID'si.";

        /// <summary>
        /// Geçersiz rol adı için kullanılan mesaj.
        /// </summary>
        public const string InvalidRoleName = "Geçersiz rol adı.";

        /// <summary>
        /// Admin rolü silinemez.
        /// </summary>
        public const string CannotDeleteAdminRole = "Admin rolü silinemez.";

        /// <summary>
        /// Rol güncellenemedi.
        /// </summary>
        /// <remarks>
        /// Rol güncelleme işlemi sırasında kullanılır.
        /// </remarks>
        public const string RoleUpdateFailed = "Rol güncellenemedi.";

        /// <summary>
        /// Rol silinemedi.
        /// </summary>
        /// <remarks>
        /// Rol silme işlemi sırasında kullanılır.
        /// </remarks>
        public const string RoleDeleteFailed = "Rol silinemedi.";

        /// <summary>
        /// Rol oluşturulamadı.
        /// </summary>
        /// <remarks>
        /// Rol oluşturma işlemi sırasında kullanılır.
        /// </remarks>
        public const string RoleCreateFailed = "Rol oluşturulamadı.";

        #endregion

        #region İzin ile İlgili Hata Mesajları

        /// <summary>
        /// Belirtilen iznin sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        public const string PermissionNotFound = "İzin bulunamadı";

        /// <summary>
        /// Belirtilen iznin sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string PermissionNotFoundById = "ID: {0} olan yetki bulunamadı.";

        /// <summary>
        /// İzin adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        /// <remarks>
        /// Kullanıcı girişi veya kullanıcı bilgisi getirme işlemlerinde kullanılır.
        /// </remarks>
        public const string DuplicatePermissionName = "Bu yetki adı zaten kullanılıyor: {0}";

        /// <summary>
        /// Geçersiz izin ID'si için kullanılan mesaj.
        /// </summary>
        public const string InvalidPermissionId = "Geçersiz yetki ID'si.";

        /// <summary>
        /// Geçersiz izin adı için kullanılan mesaj.
        /// </summary>
        public const string InvalidPermissionName = "Geçersiz yetki adı.";

        /// <summary>
        /// İzin güncellenemedi.
        /// </summary>
        /// <remarks>
        /// İzin güncelleme işlemi sırasında kullanılır.
        /// </remarks>
        public const string PermissionUpdateFailed = "Yetki güncellenemedi.";

        /// <summary>
        /// İzin silinemedi.
        /// </summary>
        /// <remarks>
        /// İzin silme işlemi sırasında kullanılır.
        /// </remarks>
        public const string PermissionDeleteFailed = "Yetki silinemedi.";

        /// <summary>
        /// İzin oluşturulamadı.
        /// </summary>
        /// <remarks>
        /// İzin oluşturma işlemi sırasında kullanılır.
        /// </remarks>
        public const string PermissionCreateFailed = "Yetki oluşturulamadı.";

        /// <summary>
        /// Kullanıcıya izin atanırken hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string UserPermissionAssignError = "Kullanıcıya izinler atanırken bir hata oluştu: {0}";

        /// <summary>
        /// Eksik izinler eklenirken hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string MissingPermissionsError = "Eksik izinler eklenirken bir hata oluştu: {0}";

        /// <summary>
        /// İzin atama işlemi sırasında hata oluştuğunda kullanılan mesaj.
        /// </summary>
        public const string PermissionAssignmentError = "İzin atama işlemi sırasında hata oluştu";

        #endregion

        #region Ürün ile İlgili Hata Mesajları

        /// <summary>
        /// Belirtilen ürünün sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        public const string ProductNotFound = "Ürün bulunamadı";

        /// <summary>
        /// Ürün oluşturma işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string ProductCreateError = "Ürün oluşturulurken bir hata oluştu: {0}";

        /// <summary>
        /// Ürün güncelleme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string ProductUpdateError = "Ürün güncellenirken bir hata oluştu: {0}";

        /// <summary>
        /// Ürün silme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string ProductDeleteError = "Ürün silinirken bir hata oluştu: {0}";

        /// <summary>
        /// Ürün adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        public const string DuplicateProductName = "Bu ürün adı zaten kullanılmaktadır";

        #endregion

        #region Kategori ile İlgili Hata Mesajları

        /// <summary>
        /// Belirtilen kategorinin sistemde bulunamadığı durumlarda kullanılan mesaj.
        /// </summary>
        public const string CategoryNotFound = "Kategori bulunamadı";

        /// <summary>
        /// Kategori oluşturma işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string CategoryCreateError = "Kategori oluşturulurken bir hata oluştu: {0}";

        /// <summary>
        /// Kategori güncelleme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string CategoryUpdateError = "Kategori güncellenirken bir hata oluştu: {0}";

        /// <summary>
        /// Kategori silme işlemi sırasında hata oluştuğunda kullanılan mesaj formatı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string CategoryDeleteError = "Kategori silinirken bir hata oluştu: {0}";

        /// <summary>
        /// Kategori adının sistemde zaten var olduğu durumlarda kullanılan mesaj.
        /// </summary>
        public const string DuplicateCategoryName = "Bu kategori adı zaten kullanılmaktadır";

        #endregion

        #region Oturum işlemleri hataları

        /// <summary>
        /// Hesabınız kilitlendi. Lütfen sistem yöneticisi ile iletişime geçin.
        /// </summary>
        public const string AccountLocked = "Hesabınız kilitlendi. Lütfen sistem yöneticisi ile iletişime geçin.";

        /// <summary>
        /// Hesabınız devre dışı bırakıldı.
        /// </summary>
        public const string AccountDisabled = "Hesabınız devre dışı bırakıldı.";

        /// <summary>
        /// Oturumunuz sona erdi. Lütfen tekrar giriş yapın.
        /// </summary>
        public const string SessionExpired = "Oturumunuz sona erdi. Lütfen tekrar giriş yapın.";

        /// <summary>
        /// Geçersiz token.
        /// </summary>
        public const string InvalidToken = "Geçersiz token.";

        /// <summary>
        /// Token süresi doldu.
        /// </summary>
        public const string TokenExpired = "Token süresi doldu.";

        #endregion

        #region Veritabanı işlemleri hataları

        /// <summary>
        /// Veritabanı bağlantı hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseConnectionError = "Veritabanı bağlantı hatası: {0}";

        /// <summary>
        /// Veritabanı sorgu hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseQueryError = "Veritabanı sorgu hatası: {0}";

        /// <summary>
        /// Veritabanı güncelleme hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseUpdateError = "Veritabanı güncelleme hatası: {0}";

        /// <summary>
        /// Veritabanı ekleme hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseInsertError = "Veritabanı ekleme hatası: {0}";

        /// <summary>
        /// Veritabanı silme hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseDeleteError = "Veritabanı silme hatası: {0}";

        /// <summary>
        /// Veritabanı eşzamanlılık hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string DatabaseConcurrencyError = "Veritabanı eşzamanlılık hatası: {0}";

        /// <summary>
        /// Veritabanı işlemi sırasında hata oluştuğunda kullanılan mesaj.
        /// </summary>
        public const string DatabaseOperationError = "Veritabanı işlemi sırasında hata oluştu";

        #endregion

        #region Dosya işlemleri hataları

        /// <summary>
        /// Dosya bulunamadı.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string FileNotFound = "Dosya bulunamadı: {0}";

        /// <summary>
        /// Dosya yükleme hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string FileUploadError = "Dosya yükleme hatası: {0}";

        /// <summary>
        /// Dosya silme hatası.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string FileDeleteError = "Dosya silme hatası: {0}";

        /// <summary>
        /// Geçersiz dosya türü.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string InvalidFileType = "Geçersiz dosya türü: {0}";

        /// <summary>
        /// Dosya boyutu çok büyük.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string FileSizeTooLarge = "Dosya boyutu çok büyük: {0}";

        #endregion

        #region Validasyon hataları

        /// <summary>
        /// Zorunlu alan.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string RequiredField = "{0} alanı zorunludur.";

        /// <summary>
        /// Geçersiz format.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// </remarks>
        public const string InvalidFormat = "{0} alanı geçersiz formatta.";

        /// <summary>
        /// String çok uzun.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// {1} parametresi ile maksimum karakter sayısı eklenir.
        /// </remarks>
        public const string StringTooLong = "{0} alanı çok uzun. Maksimum {1} karakter olmalıdır.";

        /// <summary>
        /// String çok kısa.
        /// </summary>
        /// <remarks>
        /// {0} parametresi ile spesifik hata detayı eklenir.
        /// {1} parametresi ile minimum karakter sayısı eklenir.
        /// </remarks>
        public const string StringTooShort = "{0} alanı çok kısa. Minimum {1} karakter olmalıdır.";

        /// <summary>
        /// Geçersiz e-posta adresi.
        /// </summary>
        public const string InvalidEmail = "Geçersiz e-posta adresi.";

        /// <summary>
        /// Geçersiz telefon numarası.
        /// </summary>
        public const string InvalidPhoneNumber = "Geçersiz telefon numarası.";

        /// <summary>
        /// Geçersiz tarih.
        /// </summary>
        public const string InvalidDate = "Geçersiz tarih.";

        /// <summary>
        /// Tarih aralığında olmalıdır.
        /// </summary>
        /// <remarks>
        /// {0} ve {1} parametreleri ile tarih aralığı eklenir.
        /// </remarks>
        public const string DateOutOfRange = "Tarih {0} - {1} aralığında olmalıdır.";

        /// <summary>
        /// Sayı aralığında olmalıdır.
        /// </summary>
        /// <remarks>
        /// {0} ve {1} parametreleri ile sayı aralığı eklenir.
        /// </remarks>
        public const string NumberOutOfRange = "Sayı {0} - {1} aralığında olmalıdır.";

        /// <summary>
        /// Maksimum uzunluk hatası için kullanılan mesaj formatı.
        /// </summary>
        public const string MaxLengthError = "{0} alanı en fazla {1} karakter olmalıdır";

        /// <summary>
        /// Minimum uzunluk hatası için kullanılan mesaj formatı.
        /// </summary>
        public const string MinLengthError = "{0} alanı en az {1} karakter olmalıdır";

        #endregion

        #region Şifreleme Hata Mesajları
        public const string PasswordCannotBeNullOrEmpty = "Şifre boş olamaz.";
        public const string PasswordHashingFailed = "Şifre hashleme işlemi başarısız oldu.";
        #endregion

        public const string PasswordVerificationFailed = "Şifre doğrulaması başarısız.";
        public const string UsernameExists = "Bu kullanıcı adı zaten kullanılıyor.";
        public const string RegistrationFailed = "Kayıt işlemi başarısız oldu.";
        public const string IncorrectPassword = "Yanlış şifre girdiniz.";
        public const string ChangePasswordFailed = "Şifre değiştirme işlemi başarısız oldu.";
    }
} 