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

        /// <summary>
        /// İsme göre izin arama - performans için optimize edildi
        /// </summary>
        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        /// <summary>
        /// Gruba göre izinleri getirme - performans için optimize edildi
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionsByGroupAsync(string group)
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Where(p => p.Group == group)
                .OrderBy(p => p.Name) // İsme göre sırala
                .ToListAsync();
        }

        /// <summary>
        /// Rol ID'sine göre izinleri getirme - performans için optimize edildi
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .OrderBy(p => p.Group) // Gruba göre sırala
                .ThenBy(p => p.Name) // Sonra isme göre sırala
                .ToListAsync();
        }

        /// <summary>
        /// Rol izinlerini kaldırma
        /// </summary>
        public async Task RemoveRolePermissionsAsync(int roleId)
        {
            var existingRolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
            
            _context.RolePermissions.RemoveRange(existingRolePermissions);
        }
        
        /// <summary>
        /// Rol izinlerini ekleme
        /// </summary>
        public async Task AddRolePermissionsAsync(int roleId, List<int> permissionIds)
        {
            // Toplu ekleme için liste oluştur
            var rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            }).ToList();
            
            // Toplu ekleme yap
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
        }
        
        /// <summary>
        /// Sayfalama ve filtreleme ile izinleri getirme
        /// </summary>
        public async Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetPaginatedPermissionsAsync(
            int pageNumber, 
            int pageSize, 
            string nameFilter = null,
            string groupFilter = null,
            string resourceTypeFilter = null,
            string sortBy = "Name",
            bool sortAscending = true)
        {
            // Filtreleme için IQueryable oluştur
            var query = _context.Permissions.AsQueryable();
            
            // Filtreleri uygula
            if (!string.IsNullOrWhiteSpace(nameFilter))
                query = query.Where(p => p.Name.Contains(nameFilter));
                
            if (!string.IsNullOrWhiteSpace(groupFilter))
                query = query.Where(p => p.Group == groupFilter);
                
            if (!string.IsNullOrWhiteSpace(resourceTypeFilter))
                query = query.Where(p => p.ResourceType == resourceTypeFilter);
            
            // Toplam kayıt sayısını hesapla - AsNoTracking() ile performans artışı
            var totalCount = await query.AsNoTracking().CountAsync();
            
            // Sıralama uygula
            query = sortBy?.ToLower() switch
            {
                "id" => sortAscending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id),
                "group" => sortAscending ? query.OrderBy(p => p.Group) : query.OrderByDescending(p => p.Group),
                "resourcetype" => sortAscending ? query.OrderBy(p => p.ResourceType) : query.OrderByDescending(p => p.ResourceType),
                "description" => sortAscending ? query.OrderBy(p => p.Description) : query.OrderByDescending(p => p.Description),
                _ => sortAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name) // Varsayılan sıralama
            };
            
            // Sayfalama uygula
            var permissions = await query
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (permissions, totalCount);
        }
        
        /// <summary>
        /// Tüm izin gruplarını getirme
        /// </summary>
        public async Task<IEnumerable<string>> GetAllPermissionGroupsAsync()
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Select(p => p.Group)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
        }
        
        /// <summary>
        /// Tüm kaynak tiplerini getirme
        /// </summary>
        public async Task<IEnumerable<string>> GetAllResourceTypesAsync()
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Select(p => p.ResourceType)
                .Distinct()
                .OrderBy(rt => rt)
                .ToListAsync();
        }
        
        /// <summary>
        /// Tüm izinleri getirme - performans için optimize edildi
        /// </summary>
        public override async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .OrderBy(p => p.Group) // Gruba göre sırala
                .ThenBy(p => p.Name) // Sonra isme göre sırala
                .ToListAsync();
        }
        
        /// <summary>
        /// ID'ye göre izin getirme - performans için optimize edildi
        /// </summary>
        public override async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
} 