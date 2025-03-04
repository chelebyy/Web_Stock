using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _unitOfWork.GetRepository<Permission>().GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Group = p.Group
            }).ToList();
        }

        public async Task<List<PermissionGroupDto>> GetPermissionsByGroupsAsync()
        {
            var permissions = await _unitOfWork.GetRepository<Permission>().GetAllAsync();
            return permissions
                .GroupBy(p => p.Group)
                .Select(g => new PermissionGroupDto
                {
                    Group = g.Key,
                    Permissions = g.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Group = p.Group
                    }).ToList()
                }).ToList();
        }

        public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission>() as IPermissionRepository;
            var permissions = await permissionRepo.GetPermissionsByRoleIdAsync(roleId);
            
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Group = p.Group
            }).ToList();
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                // Önce mevcut rol-izin ilişkilerini temizle
                var existingRolePermissions = await _unitOfWork.Context.Set<RolePermission>()
                    .Where(rp => rp.RoleId == roleId)
                    .ToListAsync();
                
                _unitOfWork.Context.Set<RolePermission>().RemoveRange(existingRolePermissions);
                
                // Yeni rol-izin ilişkilerini ekle
                foreach (var permissionId in permissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    };
                    
                    await _unitOfWork.Context.Set<RolePermission>().AddAsync(rolePermission);
                }
                
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            
            if (user == null || user.RoleId == null)
                return false;
                
            // Admin her zaman tüm izinlere sahiptir
            if (user.IsAdmin)
                return true;
                
            var permissionRepo = _unitOfWork.GetRepository<Permission>() as IPermissionRepository;
            var permissions = await permissionRepo.GetPermissionsByRoleIdAsync(user.RoleId.Value);
            
            return permissions.Any(p => p.Name == permissionName);
        }
    }
} 