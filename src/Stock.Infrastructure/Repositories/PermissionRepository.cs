using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByGroupAsync(string group)
        {
            return await _context.Permissions
                .Where(p => p.Group == group)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task RemoveRolePermissionsAsync(int roleId)
        {
            var existingRolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
            
            _context.RolePermissions.RemoveRange(existingRolePermissions);
        }
        
        public async Task AddRolePermissionsAsync(int roleId, List<int> permissionIds)
        {
            foreach (var permissionId in permissionIds)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                };
                
                await _context.RolePermissions.AddAsync(rolePermission);
            }
        }
    }
} 