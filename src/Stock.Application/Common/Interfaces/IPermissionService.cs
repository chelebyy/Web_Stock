using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<List<PermissionGroupDto>> GetPermissionsByGroupsAsync();
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId);
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task<bool> HasPermissionAsync(int userId, string permissionName);
    }
} 