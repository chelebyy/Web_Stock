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
        Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId);
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted = true);
        Task<bool> RemoveUserPermissionAsync(int userId, int permissionId);
        Task<bool> ResetUserToRolePermissionsAsync(int userId);
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
    }
} 