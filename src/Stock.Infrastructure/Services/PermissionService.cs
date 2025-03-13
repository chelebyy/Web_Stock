using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            IPermissionRepository permissionRepository,
            ApplicationDbContext context,
            ILogger<PermissionService> logger)
        {
            _permissionRepository = permissionRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            try
            {
                var permissions = await _permissionRepository.GetAllAsync();
                return permissions.Select(p => MapToDto(p)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all permissions");
                return new List<PermissionDto>();
            }
        }

        public async Task<List<PermissionGroupDto>> GetPermissionsByGroupsAsync()
        {
            try
            {
                var permissions = await _permissionRepository.GetAllAsync();
                
                // Grup bazında izinleri düzenle
                var groupedPermissions = permissions
                    .GroupBy(p => p.Group ?? "Diğer")
                    .Select(g => new PermissionGroupDto
                    {
                        Group = g.Key,
                        Permissions = g.Select(p => MapToDto(p)).ToList()
                    })
                    .OrderBy(g => g.Group)
                    .ToList();

                return groupedPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions by groups");
                return new List<PermissionGroupDto>();
            }
        }

        public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId)
        {
            try
            {
                var rolePermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .Include(rp => rp.Permission)
                    .ToListAsync();

                return rolePermissions
                    .Where(rp => rp.Permission != null)
                    .Select(rp => MapToDto(rp.Permission))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions by role ID {RoleId}", roleId);
                return new List<PermissionDto>();
            }
        }

        public async Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId)
        {
            try
            {
                // Kullanıcıyı ve rolünü al
                var user = await _context.Users
                    .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return new List<PermissionDto>();

                // Rol izinlerini DTO'ya dönüştür
                var roleDtos = new List<PermissionDto>();
                if (user.Role?.RolePermissions != null)
                {
                    foreach (var rp in user.Role.RolePermissions)
                    {
                        if (rp.Permission != null)
                        {
                            roleDtos.Add(new PermissionDto
                            {
                                Id = rp.Permission.Id,
                                Name = rp.Permission.Name,
                                Description = rp.Permission.Description,
                                ResourceType = rp.Permission.ResourceType ?? "",
                                Group = rp.Permission.Group,
                                IsFromRole = true,
                                IsCustom = false,
                                RoleName = user.Role.Name
                            });
                        }
                    }
                }

                // Kullanıcıya özel izinleri al
                var userPermissions = await _context.UserPermissions
                    .Where(up => up.UserId == userId && up.IsGranted)
                    .Include(up => up.Permission)
                    .ToListAsync();

                // Kullanıcı izinlerini DTO'ya dönüştür
                var userDtos = userPermissions
                    .Where(up => up.Permission != null)
                    .Select(up => new PermissionDto
                    {
                        Id = up.Permission.Id,
                        Name = up.Permission.Name,
                        Description = up.Permission.Description,
                        ResourceType = up.Permission.ResourceType ?? "",
                        Group = up.Permission.Group,
                        IsFromRole = false,
                        IsCustom = true
                    })
                    .ToList();

                // Tüm izinleri birleştir ve döndür
                return roleDtos.Concat(userDtos).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions by user ID {UserId}", userId);
                return new List<PermissionDto>();
            }
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                await _permissionRepository.RemoveRolePermissionsAsync(roleId);

                if (permissionIds.Count > 0)
                {
                    await _permissionRepository.AddRolePermissionsAsync(roleId, permissionIds);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permissions to role {RoleId}", roleId);
                return false;
            }
        }

        public async Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted)
        {
            try
            {
                // Kullanıcı izni zaten var mı kontrol et
                var existingPermission = await _context.UserPermissions
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

                if (existingPermission != null)
                {
                    // Mevcut izni güncelle
                    existingPermission.IsGranted = isGranted;
                    existingPermission.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    // Yeni izin oluştur
                    var userPermission = new UserPermission
                    {
                        UserId = userId,
                        PermissionId = permissionId,
                        IsGranted = isGranted,
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

                if (userPermission != null)
                {
                    _context.UserPermissions.Remove(userPermission);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission {PermissionId} from user {UserId}", permissionId, userId);
                return false;
            }
        }

        public async Task<bool> ResetUserToRolePermissionsAsync(int userId)
        {
            try
            {
                var userPermissions = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .ToListAsync();

                _context.UserPermissions.RemoveRange(userPermissions);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting permissions for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            try
            {
                // Admin kullanıcılar her zaman tüm izinlere sahiptir
                var isAdmin = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.IsAdmin)
                    .FirstOrDefaultAsync();

                if (isAdmin)
                    return true;

                // İzin adına göre izin ID'sini bul
                var permission = await _permissionRepository.GetByNameAsync(permissionName);
                if (permission == null)
                    return false;

                // Kullanıcının rolüne bağlı izinleri kontrol et
                var user = await _context.Users
                    .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user?.Role?.RolePermissions.Any(rp => rp.PermissionId == permission.Id) == true)
                    return true;

                // Kullanıcıya özel izinleri kontrol et
                var hasCustomPermission = await _context.UserPermissions
                    .AnyAsync(up => up.UserId == userId && up.PermissionId == permission.Id && up.IsGranted);

                return hasCustomPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} has permission {PermissionName}", userId, permissionName);
                return false;
            }
        }

        private PermissionDto MapToDto(Permission permission, bool isFromRole = false)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                ResourceType = permission.ResourceType ?? "",
                Group = permission.Group,
                IsFromRole = isFromRole,
                IsCustom = !isFromRole
            };
        }
    }
} 