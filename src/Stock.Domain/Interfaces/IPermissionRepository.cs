using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// İzin (Permission) entity'si için repository arayüzü.
    /// Temel CRUD işlemleri IRepository<Permission> arayüzünden gelir.
    /// </summary>
    public interface IPermissionRepository : IRepository<Permission>
    {
        // Spesifik metotlar kaldırıldı. Sorgular Specification Pattern ile yapılacak.
        // RolePermission ile ilgili işlemler (GetPermissionsByRoleIdAsync, RemoveRolePermissionsAsync, AddRolePermissionsAsync) kaldırıldı.
        // Bu işlemler ilgili Service/Handler katmanında IUnitOfWork kullanılarak yapılmalı.
    }
} 