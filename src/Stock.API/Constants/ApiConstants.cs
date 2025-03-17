namespace Stock.API.Constants
{
    /// <summary>
    /// API ile ilgili sabit değerleri içeren sınıf
    /// </summary>
    public static class ApiConstants
    {
        // API endpoint'leri
        public const string ApiBaseRoute = "api";
        public const string AuthRoute = "Auth";
        public const string UsersRoute = "Users";
        public const string RolesRoute = "Roles";
        public const string PermissionsRoute = "Permissions";
        public const string ProductsRoute = "Products";
        public const string CategoriesRoute = "Categories";
        
        // API yanıt mesajları
        public const string SuccessMessage = "İşlem başarıyla tamamlandı";
        public const string CreatedMessage = "Kayıt başarıyla oluşturuldu";
        public const string UpdatedMessage = "Kayıt başarıyla güncellendi";
        public const string DeletedMessage = "Kayıt başarıyla silindi";
        
        // Sayfalama parametreleri
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int DefaultPageNumber = 1;
    }
} 