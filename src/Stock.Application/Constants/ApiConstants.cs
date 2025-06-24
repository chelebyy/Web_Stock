namespace Stock.Application.Constants
{
    /// <summary>
    /// API ile ilgili sabit değerleri içeren sınıf.
    /// Bu sınıf, API endpoint'leri, yanıt mesajları ve sayfalama parametreleri gibi
    /// uygulama genelinde kullanılan sabit değerleri tanımlar.
    /// </summary>
    /// <remarks>
    /// Bu sınıftaki sabitler, API'nin tutarlı bir şekilde yapılandırılmasını ve
    /// yönetilmesini sağlar. Tüm controller'lar ve servisler bu sabitleri kullanmalıdır.
    /// </remarks>
    public static class ApiConstants
    {
        #region API Endpoint Sabitleri
        
        /// <summary>
        /// API'nin temel yolu. Tüm endpoint'ler bu yol altında tanımlanır.
        /// </summary>
        /// <example>
        /// Kullanım örneği: [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
        /// </example>
        public const string ApiBaseRoute = "api";

        /// <summary>
        /// Kimlik doğrulama ve yetkilendirme işlemleri için kullanılan endpoint.
        /// </summary>
        /// <remarks>
        /// Login, register ve şifre değiştirme gibi işlemler bu endpoint altında yapılır.
        /// </remarks>
        public const string AuthRoute = "Auth";

        /// <summary>
        /// Kullanıcı yönetimi işlemleri için kullanılan endpoint.
        /// </summary>
        public const string UsersRoute = "Users";

        /// <summary>
        /// Rol yönetimi işlemleri için kullanılan endpoint.
        /// </summary>
        public const string RolesRoute = "Roles";

        /// <summary>
        /// İzin yönetimi işlemleri için kullanılan endpoint.
        /// </summary>
        public const string PermissionsRoute = "Permissions";

        /// <summary>
        /// Ürün yönetimi işlemleri için kullanılan endpoint.
        /// </summary>
        public const string ProductsRoute = "Products";

        /// <summary>
        /// Kategori yönetimi işlemleri için kullanılan endpoint.
        /// </summary>
        public const string CategoriesRoute = "Categories";
        
        #endregion

        #region API Yanıt Mesajları

        /// <summary>
        /// İşlemin başarıyla tamamlandığını belirten genel başarı mesajı.
        /// </summary>
        /// <remarks>
        /// HTTP 200 OK yanıtlarında kullanılır.
        /// </remarks>
        public const string SuccessMessage = "İşlem başarıyla tamamlandı";

        /// <summary>
        /// Yeni bir kaydın başarıyla oluşturulduğunu belirten mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 201 Created yanıtlarında kullanılır.
        /// </remarks>
        public const string CreatedMessage = "Kayıt başarıyla oluşturuldu";

        /// <summary>
        /// Mevcut bir kaydın başarıyla güncellendiğini belirten mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 200 OK yanıtlarında kullanılır.
        /// </remarks>
        public const string UpdatedMessage = "Kayıt başarıyla güncellendi";

        /// <summary>
        /// Bir kaydın başarıyla silindiğini belirten mesaj.
        /// </summary>
        /// <remarks>
        /// HTTP 200 OK yanıtlarında kullanılır.
        /// </remarks>
        public const string DeletedMessage = "Kayıt başarıyla silindi";
        
        #endregion

        #region Sayfalama Parametreleri

        /// <summary>
        /// Varsayılan sayfa boyutu.
        /// </summary>
        /// <remarks>
        /// Sayfa boyutu belirtilmediğinde kullanılır.
        /// </remarks>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Varsayılan sayfa numarası.
        /// </summary>
        /// <remarks>
        /// Sayfa numarası belirtilmediğinde kullanılır.
        /// </remarks>
        public const int DefaultPageNumber = 1;

        /// <summary>
        /// İzin verilen maksimum sayfa boyutu.
        /// </summary>
        /// <remarks>
        /// Performans ve güvenlik nedeniyle sayfa boyutu bu değerle sınırlandırılmıştır.
        /// </remarks>
        public const int MaxPageSize = 100;
        
        #endregion
    }
} 