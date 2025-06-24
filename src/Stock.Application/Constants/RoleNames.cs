namespace Stock.Application.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan standart rol adlarını içeren sınıf.
    /// Bu roller, kullanıcıların sistemdeki yetki seviyelerini belirler.
    /// </summary>
    public static class RoleNames
    {
        /// <summary>
        /// Yönetici rolü. Tüm sistem üzerinde tam yetkiye sahiptir.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Standart kullanıcı rolü. Genellikle temel işlemleri yapma yetkisine sahiptir.
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// Yönetici veya Müdür rolü. Belirli bir departman veya modül üzerinde yönetim yetkisine sahiptir.
        /// </summary>
        public const string Manager = "Manager";

        /// <summary>
        /// Gözetmen veya Süpervizör rolü. Belirli işlemleri denetleme veya onaylama yetkisine sahiptir.
        /// </summary>
        public const string Supervisor = "Supervisor";

        /// <summary>
        /// Sadece okuma yetkisine sahip rol. Verileri görüntüleyebilir ancak değişiklik yapamaz.
        /// </summary>
        public const string ReadOnly = "ReadOnly";

        /// <summary>
        /// Revir modülüne özel kullanıcı rolü.
        /// </summary>
        public const string RevirUser = "RevirUser";

        /// <summary>
        /// Bilgi İşlem modülüne özel kullanıcı rolü.
        /// </summary>
        public const string BilgiIslemUser = "BilgiIslemUser";

        /// <summary>
        /// Temel kullanıcı rolü. Genellikle temel işlemleri yapma yetkisine sahiptir.
        /// </summary>
        public const string Basic = "Basic";
    }
} 