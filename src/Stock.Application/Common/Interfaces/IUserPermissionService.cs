using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    public interface IUserPermissionService
    {
        Task<List<PermissionDto>> GetUserPermissionsAsync(int userId);
        Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted);
        Task<bool> RemoveUserPermissionAsync(int userId, int permissionId);
        Task<bool> HasPermissionAsync(int userId, string permissionName);
        Task<List<PermissionDto>> GetUserCustomPermissionsAsync(int userId);
        Task<bool> ResetUserToRolePermissionsAsync(int userId);
    }
} 