using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// İsme göre rol arama - performans için optimize edildi
        /// </summary>
        public virtual async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        /// <summary>
        /// Kullanıcıları içeren rolleri getirme - performans için optimize edildi
        /// </summary>
        public virtual async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Include(r => r.Users.Where(u => !u.IsDeleted)) // Sadece silinmemiş kullanıcıları dahil et
                .ToListAsync();
        }
        
        /// <summary>
        /// ID'ye göre rol getirme - performans için optimize edildi
        /// </summary>
        public override async Task<Role?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        
        /// <summary>
        /// Tüm rolleri getirme - performans için optimize edildi
        /// </summary>
        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Where(r => !r.IsDeleted) // Sadece silinmemiş rolleri getir
                .OrderBy(r => r.Name) // İsme göre sırala
                .ToListAsync();
        }
        
        /// <summary>
        /// Rol ekleme
        /// </summary>
        public override async Task AddAsync(Role entity)
        {
            await base.AddAsync(entity);
        }
        
        /// <summary>
        /// Rol güncelleme
        /// </summary>
        public override async Task UpdateAsync(Role entity)
        {
            await base.UpdateAsync(entity);
        }
        
        /// <summary>
        /// Rol silme (soft delete)
        /// </summary>
        public override async Task DeleteAsync(Role entity)
        {
            await base.DeleteAsync(entity);
        }
        
        /// <summary>
        /// Sayfalama ve filtreleme ile rolleri getirme
        /// </summary>
        public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetPaginatedRolesAsync(
            int pageNumber, 
            int pageSize, 
            string nameFilter = null,
            bool includeUsers = false,
            string sortBy = "Name",
            bool sortAscending = true)
        {
            // Filtreleme için IQueryable oluştur
            var query = _dbSet.AsQueryable();
            
            // Filtreleri uygula
            if (!string.IsNullOrWhiteSpace(nameFilter))
                query = query.Where(r => r.Name.Contains(nameFilter));
                
            // Sadece silinmemiş rolleri getir
            query = query.Where(r => !r.IsDeleted);
            
            // Toplam kayıt sayısını hesapla - AsNoTracking() ile performans artışı
            var totalCount = await query.AsNoTracking().CountAsync();
            
            // Sıralama uygula
            query = sortBy?.ToLower() switch
            {
                "id" => sortAscending ? query.OrderBy(r => r.Id) : query.OrderByDescending(r => r.Id),
                "description" => sortAscending ? query.OrderBy(r => r.Description) : query.OrderByDescending(r => r.Description),
                "createdat" => sortAscending ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
                _ => sortAscending ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name) // Varsayılan sıralama
            };
            
            // Kullanıcıları dahil et
            if (includeUsers)
                query = query.Include(r => r.Users.Where(u => !u.IsDeleted));
            
            // Sayfalama uygula
            var roles = await query
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (roles, totalCount);
        }
    }
} 