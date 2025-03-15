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
        
        /// <summary>
        /// Sayfalama ve filtreleme ile izinleri getirme
        /// </summary>
        Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetPaginatedPermissionsAsync(
            int pageNumber, 
            int pageSize, 
            string nameFilter = null,
            string groupFilter = null,
            string resourceTypeFilter = null,
            string sortBy = "Name",
            bool sortAscending = true);
            
        /// <summary>
        /// Tüm izin gruplarını getirme
        /// </summary>
        Task<IEnumerable<string>> GetAllPermissionGroupsAsync();
        
        /// <summary>
        /// Tüm kaynak tiplerini getirme
        /// </summary>
        Task<IEnumerable<string>> GetAllResourceTypesAsync();
    }
} 