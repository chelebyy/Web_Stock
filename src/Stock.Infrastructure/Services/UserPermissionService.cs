using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserPermissionService> _logger;

        public UserPermissionService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<UserPermissionService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<List<PermissionDto>> GetUserPermissionsAsync(int userId)
        {
            try
            {
                // Get user's role permissions
                var user = await _context.Users
                    .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new List<PermissionDto>();
                }

                var rolePermissions = user.Role.RolePermissions
                    .Select(rp => rp.Permission)
                    .ToList();

                // Get user's custom permissions
                var userPermissions = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .Include(up => up.Permission)
                    .ToListAsync();

                // Create a dictionary of all permissions from role
                var permissionsDictionary = rolePermissions
                    .ToDictionary(p => p.Id, p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Group = p.Group,
                        ResourceType = p.ResourceType,
                        ResourceName = p.ResourceName,
                        Action = p.Action,
                        IsFromRole = true,
                        RoleName = user.Role.Name
                    });

                // Override or add custom user permissions
                foreach (var userPermission in userPermissions)
                {
                    var permission = userPermission.Permission;

                    // Skip if the permission is explicitly denied
                    if (!userPermission.IsGranted)
                    {
                        permissionsDictionary.Remove(permission.Id);
                        continue;
                    }

                    // Add or update the permission
                    permissionsDictionary[permission.Id] = new PermissionDto
                    {
                        Id = permission.Id,
                        Name = permission.Name,
                        Description = permission.Description,
                        Group = permission.Group,
                        ResourceType = permission.ResourceType,
                        ResourceName = permission.ResourceName,
                        Action = permission.Action,
                        IsFromRole = false,
                        IsCustom = true
                    };
                }

                return permissionsDictionary.Values.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user permissions for user ID {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted)
        {
            try
            {
                // Check if user and permission exist
                var user = await _context.Users.FindAsync(userId);
                var permission = await _context.Permissions.FindAsync(permissionId);

                if (user == null || permission == null)
                {
                    return false;
                }

                // Check if the user already has a custom permission entry
                var existingPermission = await _context.UserPermissions
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

                if (existingPermission != null)
                {
                    // Update existing permission
                    existingPermission.IsGranted = isGranted;
                    existingPermission.LastModifiedBy = _currentUserService.UserId?.ToString();
                    existingPermission.LastModifiedAt = DateTime.UtcNow;
                }
                else
                {
                    // Create new permission entry
                    var userPermission = new UserPermission
                    {
                        UserId = userId,
                        PermissionId = permissionId,
                        IsGranted = isGranted,
                        CreatedBy = _currentUserService.UserId?.ToString(),
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.UserPermissions.AddAsync(userPermission);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permission {PermissionId} to user {UserId}", permissionId, userId);
                return false;
            }
        }

        public async Task<bool> RemoveUserPermissionAsync(int userId, int permissionId)
        {
            try
            {
                var userPermission = await _context.UserPermissions
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

                if (userPermission == null)
                {
                    return false;
                }

                _context.UserPermissions.Remove(userPermission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission {PermissionId} from user {UserId}", permissionId, userId);
                return false;
            }
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            try
            {
                // Get user's permissions including role permissions and custom user permissions
                var userPermissions = await GetUserPermissionsAsync(userId);
                return userPermissions.Any(p => p.Name == permissionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} has permission {PermissionName}", userId, permissionName);
                return false;
            }
        }

        public async Task<List<UserPermissionDto>> GetUserCustomPermissionsAsync(int userId)
        {
            try
            {
                var userPermissions = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .Include(up => up.Permission)
                    .Include(up => up.User)
                    .Select(up => new UserPermissionDto
                    {
                        Id = up.Id,
                        UserId = up.UserId,
                        PermissionId = up.PermissionId,
                        IsGranted = up.IsGranted,
                        UserName = up.User.Username,
                        PermissionName = up.Permission.Name,
                        ResourceType = up.Permission.ResourceType,
                        ResourceName = up.Permission.ResourceName,
                        Action = up.Permission.Action,
                        Description = up.Permission.Description,
                        Group = up.Permission.Group
                    })
                    .ToListAsync();

                return userPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting custom permissions for user {UserId}", userId);
                return new List<UserPermissionDto>();
            }
        }

        public async Task<bool> ResetUserToRolePermissionsAsync(int userId)
        {
            try
            {
                var userPermissions = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .ToListAsync();

                if (userPermissions.Any())
                {
                    _context.UserPermissions.RemoveRange(userPermissions);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting user {UserId} to role permissions", userId);
                return false;
            }
        }
    }
} 