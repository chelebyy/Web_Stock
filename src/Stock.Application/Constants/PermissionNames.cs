using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stock.Application.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan izin adlarını içeren sınıf.
    /// Bu sınıf, uygulamanın farklı bölümlerine erişim izinlerini tanımlar.
    /// </summary>
    /// <remarks>
    /// İzinler kategorilere ayrılmıştır:
    /// - Genel izinler
    /// - Kullanıcı izinleri
    /// - Rol izinleri
    /// - İzin izinleri
    /// - Kullanıcı izinleri yönetimi
    /// - Ürün izinleri
    /// - Kategori izinleri
    /// - Sayfa erişim izinleri
    /// </remarks>
    public static class PermissionNames
    {
        #region Genel İzinler

        /// <summary>
        /// Tüm sistem üzerinde tam yetkiye sahip admin izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar tüm işlemleri gerçekleştirebilir.
        /// </remarks>
        public const string Admin = "Admin";

        /// <summary>
        /// Bilgi İşlem sayfasına erişim izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, Bilgi İşlem modülüne erişebilir.
        /// </remarks>
        public const string PagesITAccess = "Pages.IT.Access";

        /// <summary>
        /// Revir sayfasını görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, Revir modülünü görüntüleyebilir.
        /// </remarks>
        public const string PagesRevirView = "Pages.Revir.View";

        /// <summary>
        /// Bilgi İşlem sayfasını görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, Bilgi İşlem modülünü görüntüleyebilir.
        /// </remarks>
        public const string PagesBilgiIslemView = "Pages.BilgiIslem.View";

        /// <summary>
        /// Kontrol paneli sayfasını görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, ana kontrol paneline erişebilir.
        /// </remarks>
        public const string PagesDashboardView = "Pages.Dashboard.View";

        /// <summary>
        /// Yönetim sayfasını görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, yönetim fonksiyonlarına erişebilir.
        /// </remarks>
        public const string PagesAdminView = "Pages.Admin.View";

        /// <summary>
        /// Genel izinleri görme yetkisi.
        /// </summary>
        public const string ViewDashboard = "Permissions.General.ViewDashboard";

        #endregion

        #region Kullanıcı İzinleri

        /// <summary>
        /// Kullanıcıları görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki kullanıcıları listeleyebilir ve detaylarını görebilir.
        /// </remarks>
        public const string UsersView = "Permissions.Users.View";

        /// <summary>
        /// Yeni kullanıcı oluşturma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sisteme yeni kullanıcı ekleyebilir.
        /// </remarks>
        public const string UsersCreate = "Permissions.Users.Create";

        /// <summary>
        /// Kullanıcı bilgilerini güncelleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, mevcut kullanıcıların bilgilerini güncelleyebilir.
        /// </remarks>
        public const string UsersEdit = "Permissions.Users.Edit";

        /// <summary>
        /// Kullanıcı silme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemden kullanıcı silebilir.
        /// </remarks>
        public const string UsersDelete = "Permissions.Users.Delete";

        #endregion

        #region Rol İzinleri

        /// <summary>
        /// Rolleri görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki rolleri listeleyebilir ve detaylarını görebilir.
        /// </remarks>
        public const string RolesView = "Permissions.Roles.View";

        /// <summary>
        /// Yeni rol oluşturma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sisteme yeni rol ekleyebilir.
        /// </remarks>
        public const string RolesCreate = "Permissions.Roles.Create";

        /// <summary>
        /// Rol bilgilerini güncelleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, mevcut rollerin bilgilerini güncelleyebilir.
        /// </remarks>
        public const string RolesEdit = "Permissions.Roles.Edit";

        /// <summary>
        /// Rol silme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemden rol silebilir.
        /// </remarks>
        public const string RolesDelete = "Permissions.Roles.Delete";

        /// <summary>
        /// Rol İzin Atama.
        /// </summary>
        public const string RolesAssignPermissions = "Permissions.Roles.AssignPermissions";

        #endregion

        #region İzin İzinleri

        /// <summary>
        /// İzinleri görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki izinleri listeleyebilir ve detaylarını görebilir.
        /// </remarks>
        public const string PermissionsView = "Permissions.Permissions.View";

        /// <summary>
        /// Yeni izin oluşturma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sisteme yeni izin ekleyebilir.
        /// </remarks>
        public const string PermissionsCreate = "Permissions.Permissions.Create";

        /// <summary>
        /// İzin bilgilerini güncelleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, mevcut izinlerin bilgilerini güncelleyebilir.
        /// </remarks>
        public const string PermissionsUpdate = "Permissions.Permissions.Update";

        /// <summary>
        /// İzin silme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemden izin silebilir.
        /// </remarks>
        public const string PermissionsDelete = "Permissions.Permissions.Delete";

        /// <summary>
        /// İzin kontrolü yapma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, diğer kullanıcıların izinlerini kontrol edebilir.
        /// </remarks>
        public const string PermissionsCheck = "Permissions.Permissions.Check";

        #endregion

        #region Kullanıcı İzinleri Yönetimi

        /// <summary>
        /// Kullanıcılara izin atama izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, diğer kullanıcılara izin atayabilir.
        /// </remarks>
        public const string UsersPermissionsAssign = "Permissions.Users.AssignPermissions";

        /// <summary>
        /// Kullanıcılardan izin kaldırma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, diğer kullanıcılardan izin kaldırabilir.
        /// </remarks>
        public const string UsersPermissionsRemove = "Permissions.Users.RemovePermissions";

        /// <summary>
        /// Kullanıcı izinlerini sıfırlama izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, diğer kullanıcıların tüm izinlerini sıfırlayabilir.
        /// </remarks>
        public const string UsersPermissionsReset = "Permissions.Users.ResetPermissions";

        #endregion

        #region Ürün İzinleri

        /// <summary>
        /// Ürünleri görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki ürünleri listeleyebilir ve detaylarını görebilir.
        /// </remarks>
        public const string ProductsView = "Permissions.Products.View";

        /// <summary>
        /// Yeni ürün oluşturma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sisteme yeni ürün ekleyebilir.
        /// </remarks>
        public const string ProductsCreate = "Permissions.Products.Create";

        /// <summary>
        /// Ürün bilgilerini güncelleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, mevcut ürünlerin bilgilerini güncelleyebilir.
        /// </remarks>
        public const string ProductsUpdate = "Permissions.Products.Update";

        /// <summary>
        /// Ürün silme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemden ürün silebilir.
        /// </remarks>
        public const string ProductsDelete = "Permissions.Products.Delete";

        #endregion

        #region Kategori İzinleri

        /// <summary>
        /// Kategorileri görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki kategorileri listeleyebilir ve detaylarını görebilir.
        /// </remarks>
        public const string CategoriesView = "Permissions.Categories.View";

        /// <summary>
        /// Yeni kategori oluşturma izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sisteme yeni kategori ekleyebilir.
        /// </remarks>
        public const string CategoriesCreate = "Permissions.Categories.Create";

        /// <summary>
        /// Kategori bilgilerini güncelleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, mevcut kategorilerin bilgilerini güncelleyebilir.
        /// </remarks>
        public const string CategoriesUpdate = "Permissions.Categories.Update";

        /// <summary>
        /// Kategori silme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemden kategori silebilir.
        /// </remarks>
        public const string CategoriesDelete = "Permissions.Categories.Delete";

        #endregion

        #region Audit Log İzinleri

        /// <summary>
        /// Audit logları görüntüleme izni.
        /// </summary>
        /// <remarks>
        /// Bu izne sahip kullanıcılar, sistemdeki audit logları görebilir.
        /// </remarks>
        public const string AuditLogsView = "Permissions.AuditLogs.View";

        #endregion

        /// <summary>
        /// Gets a list of all defined permission constants in this class.
        /// </summary>
        /// <returns>A list of permission strings.</returns>
        public static List<string> GetAllPermissions()
        {
            return typeof(PermissionNames).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue() as string)
                .Where(s => s != null)
                .ToList()!;
        }
    }
} 