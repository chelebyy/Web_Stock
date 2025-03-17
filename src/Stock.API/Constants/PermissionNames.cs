namespace Stock.API.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan izin adlarını içeren sınıf
    /// </summary>
    public static class PermissionNames
    {
        // Genel izinler
        public const string Admin = "Admin";
        
        // Kullanıcı izinleri
        public const string UsersView = "Users.View";
        public const string UsersCreate = "Users.Create";
        public const string UsersUpdate = "Users.Update";
        public const string UsersDelete = "Users.Delete";
        
        // Rol izinleri
        public const string RolesView = "Roles.View";
        public const string RolesCreate = "Roles.Create";
        public const string RolesUpdate = "Roles.Update";
        public const string RolesDelete = "Roles.Delete";
        
        // İzin izinleri
        public const string PermissionsView = "Permissions.View";
        public const string PermissionsCreate = "Permissions.Create";
        public const string PermissionsUpdate = "Permissions.Update";
        public const string PermissionsDelete = "Permissions.Delete";
        public const string PermissionsCheck = "Permissions.Check";
        
        // Kullanıcı izinleri yönetimi
        public const string UsersPermissionsAssign = "Users.Permissions.Assign";
        public const string UsersPermissionsRemove = "Users.Permissions.Remove";
        public const string UsersPermissionsReset = "Users.Permissions.Reset";
        
        // Ürün izinleri
        public const string ProductsView = "Products.View";
        public const string ProductsCreate = "Products.Create";
        public const string ProductsUpdate = "Products.Update";
        public const string ProductsDelete = "Products.Delete";
        
        // Kategori izinleri
        public const string CategoriesView = "Categories.View";
        public const string CategoriesCreate = "Categories.Create";
        public const string CategoriesUpdate = "Categories.Update";
        public const string CategoriesDelete = "Categories.Delete";
        
        // Sayfa erişim izinleri
        public const string PagesRevirView = "Pages.Revir.View";
        public const string PagesBilgiIslemView = "Pages.BilgiIslem.View";
        public const string PagesDashboardView = "Pages.Dashboard.View";
        public const string PagesAdminView = "Pages.Admin.View";
    }
} 