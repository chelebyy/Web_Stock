using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Domain.Interfaces
{
    public interface IPermissionRepository : IRepository<Stock.Domain.Entities.Permissions.Permission>
    {
        Task<Stock.Domain.Entities.Permissions.Permission?> GetByNameAsync(string name);
        Task<IEnumerable<Stock.Domain.Entities.Permissions.Permission>> GetPermissionsByGroupAsync(string group);
        Task<IEnumerable<Stock.Domain.Entities.Permissions.Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task RemoveRolePermissionsAsync(int roleId);
        Task AddRolePermissionsAsync(int roleId, List<int> permissionIds);
    }
} 