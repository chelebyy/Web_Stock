using Stock.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Domain.Interfaces
{
    public interface IPermissionRepository : IRepository<Stock.Domain.Entities.Permission>
    {
        Task<Stock.Domain.Entities.Permission?> GetByNameAsync(string name);
        Task<IEnumerable<Stock.Domain.Entities.Permission>> GetPermissionsByGroupAsync(string group);
        Task<IEnumerable<Stock.Domain.Entities.Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task RemoveRolePermissionsAsync(int roleId);
        Task AddRolePermissionsAsync(int roleId, List<int> permissionIds);
    }
} 