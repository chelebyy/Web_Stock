using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Data.Repositories
{
    /// <summary>
    /// İzin (Permission) entity'si için repository sınıfı.
    /// GenericRepository<Permission> sınıfından türetilmiştir ve IPermissionRepository arayüzünü implemente eder.
    /// Spesifik sorgular için Specification Pattern kullanılır.
    /// </summary>
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        /// <summary>
        /// PermissionRepository sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı context'i.</param>
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
            // Base class constructor'ı çağrılır.
        }

        // GetByNameAsync, GetPermissionsByRoleIdAsync, GetPermissionsByGroupAsync,
        // RemoveRolePermissionsAsync, AddRolePermissionsAsync metotları kaldırıldı.
        // Permission sorguları ilgili Specification sınıfları ve base repository metotları ile sağlanır.
        // RolePermission işlemleri ilgili Service/Handler katmanında IUnitOfWork kullanılarak yapılmalı.
    }
} 