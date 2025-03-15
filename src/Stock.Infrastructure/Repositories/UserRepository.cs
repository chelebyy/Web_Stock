using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetBySicilAsync(string sicil)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Sicil == sicil);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Include(u => u.Role)
                .ToListAsync();
        }

        /// <summary>
        /// Sayfalama ve filtreleme için optimize edilmiş metot
        /// </summary>
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(
            int pageNumber, 
            int pageSize, 
            string usernameFilter = null, 
            string sicilFilter = null, 
            int? roleIdFilter = null, 
            bool? isActiveFilter = null, 
            bool? isAdminFilter = null,
            string sortBy = "Username",
            bool sortAscending = true)
        {
            // Filtreleme için IQueryable oluştur
            var query = _dbSet.AsQueryable();
            
            // Filtreleri uygula
            if (!string.IsNullOrWhiteSpace(usernameFilter))
                query = query.Where(u => u.Username.Contains(usernameFilter));
                
            if (!string.IsNullOrWhiteSpace(sicilFilter))
                query = query.Where(u => u.Sicil.Contains(sicilFilter));
                
            if (roleIdFilter.HasValue)
                query = query.Where(u => u.RoleId == roleIdFilter.Value);
                
            if (isActiveFilter.HasValue)
                query = query.Where(u => u.IsActive == isActiveFilter.Value);
                
            if (isAdminFilter.HasValue)
                query = query.Where(u => u.IsAdmin == isAdminFilter.Value);
            
            // Toplam kayıt sayısını hesapla - AsNoTracking() ile performans artışı
            var totalCount = await query.AsNoTracking().CountAsync();
            
            // Sıralama uygula
            query = ApplySorting(query, sortBy, sortAscending);
            
            // Sayfalama uygula ve Role'ü include et
            // Tutarlı sıralama için OrderBy eklendi
            var users = await query
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Include(u => u.Role)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (users, totalCount);
        }
        
        /// <summary>
        /// Projection için optimize edilmiş metot
        /// </summary>
        public async Task<IEnumerable<User>> GetUserSummariesAsync()
        {
            return await _dbSet
                .AsNoTracking() // Performans için tracking'i devre dışı bırak
                .Include(u => u.Role)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Sicil = u.Sicil,
                    IsAdmin = u.IsAdmin,
                    IsActive = u.IsActive,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    Role = new Role 
                    { 
                        Id = u.Role.Id, 
                        Name = u.Role.Name 
                    }
                })
                .ToListAsync();
        }
        
        // Sıralama için yardımcı metot
        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, bool sortAscending)
        {
            Expression<Func<User, object>> keySelector = sortBy?.ToLower() switch
            {
                "id" => u => u.Id,
                "username" => u => u.Username,
                "sicil" => u => u.Sicil,
                "roleid" => u => u.RoleId,
                "rolename" => u => u.Role.Name,
                "isadmin" => u => u.IsAdmin,
                "isactive" => u => u.IsActive,
                "email" => u => u.Email,
                _ => u => u.Username // Varsayılan sıralama
            };
            
            return sortAscending 
                ? query.OrderBy(keySelector) 
                : query.OrderByDescending(keySelector);
        }
    }
} 