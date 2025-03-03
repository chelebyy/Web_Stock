using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Where(rp => !rp.Permission.IsDeleted)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByGroupAsync(string group)
        {
            return await _dbSet
                .Where(p => p.Group == group && !p.IsDeleted)
                .ToListAsync();
        }
        
        public async Task RemoveRolePermissionsAsync(int roleId)
        {
            var existingRolePermissions = await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
            
            _context.Set<RolePermission>().RemoveRange(existingRolePermissions);
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
                
                await _context.Set<RolePermission>().AddAsync(rolePermission);
            }
        }
    }
} 