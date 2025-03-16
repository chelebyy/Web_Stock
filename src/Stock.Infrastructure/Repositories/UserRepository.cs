using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stock.Application.Common.Interfaces;

namespace Stock.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILoggerManager _logger;
        private const string CACHE_KEY_ALL_USERS = "ALL_USERS";
        private const string CACHE_KEY_USER_BY_ID = "USER_BY_ID_";
        private const string CACHE_KEY_USER_BY_USERNAME = "USER_BY_USERNAME_";
        private const string CACHE_KEY_USER_SUMMARIES = "USER_SUMMARIES";
        private const string CACHE_KEY_PAGINATED_USERS = "PAGINATED_USERS_";
        private const int CACHE_DURATION_MINUTES = 30;

        public UserRepository(ApplicationDbContext context, ICacheService cacheService, ILoggerManager logger) : base(context)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            string cacheKey = $"{CACHE_KEY_USER_BY_ID}{id}";
            
            return await _cacheService.GetOrSetAsync(cacheKey, async () => 
            {
                _logger.LogInfo($"Veritabanından kullanıcı getiriliyor. ID: {id}");
                return await _context.Users
                    .AsNoTracking()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }, CACHE_DURATION_MINUTES);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            string cacheKey = $"{CACHE_KEY_USER_BY_USERNAME}{username}";
            
            return await _cacheService.GetOrSetAsync(cacheKey, async () => 
            {
                _logger.LogInfo($"Veritabanından kullanıcı getiriliyor. Kullanıcı adı: {username}");
                return await _context.Users
                    .AsNoTracking()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Username == username);
            }, CACHE_DURATION_MINUTES);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _cacheService.GetOrSetAsync(CACHE_KEY_ALL_USERS, async () => 
            {
                _logger.LogInfo("Veritabanından tüm kullanıcılar getiriliyor.");
                return await _context.Users
                    .AsNoTracking()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ToListAsync();
            }, CACHE_DURATION_MINUTES);
        }

        public async Task<(IEnumerable<User> users, int totalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize, string searchTerm = null, string sortBy = null, bool ascending = true)
        {
            string cacheKey = $"{CACHE_KEY_PAGINATED_USERS}page{pageNumber}_size{pageSize}_search{searchTerm ?? "none"}_sort{sortBy ?? "none"}_{ascending}";
            
            return await _cacheService.GetOrSetAsync(cacheKey, async () => 
            {
                _logger.LogInfo($"Veritabanından sayfalanmış kullanıcılar getiriliyor. Sayfa: {pageNumber}, Boyut: {pageSize}");
                
                var query = _context.Users.AsNoTracking();

                // Arama filtresi uygula
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(u => 
                        u.Username.ToLower().Contains(searchTerm) || 
                        u.Email.ToLower().Contains(searchTerm) || 
                        u.FirstName.ToLower().Contains(searchTerm) || 
                        u.LastName.ToLower().Contains(searchTerm));
                }

                // Toplam kayıt sayısını al
                var totalCount = await query.CountAsync();

                // Sıralama uygula
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    query = ApplySorting(query, sortBy, ascending);
                }
                else
                {
                    // Varsayılan sıralama
                    query = ascending 
                        ? query.OrderBy(u => u.Username) 
                        : query.OrderByDescending(u => u.Username);
                }

                // Sayfalama uygula
                var users = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ToListAsync();

                return (users, totalCount);
            }, CACHE_DURATION_MINUTES);
        }

        public async Task<IEnumerable<object>> GetUserSummariesAsync()
        {
            return await _cacheService.GetOrSetAsync(CACHE_KEY_USER_SUMMARIES, async () => 
            {
                _logger.LogInfo("Veritabanından kullanıcı özetleri getiriliyor.");
                return await _context.Users
                    .AsNoTracking()
                    .Where(u => u.IsActive)
                    .Select(u => new
                    {
                        u.Id,
                        u.Username,
                        FullName = u.FirstName + " " + u.LastName,
                        u.Email,
                        RoleCount = u.UserRoles.Count
                    })
                    .ToListAsync();
            }, CACHE_DURATION_MINUTES);
        }

        public override async Task<User> AddAsync(User entity)
        {
            var result = await base.AddAsync(entity);
            await InvalidateUserCacheAsync();
            return result;
        }

        public override async Task UpdateAsync(User entity)
        {
            await base.UpdateAsync(entity);
            await InvalidateUserCacheAsync(entity.Id);
        }

        public override async Task DeleteAsync(User entity)
        {
            await base.DeleteAsync(entity);
            await InvalidateUserCacheAsync(entity.Id);
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, bool ascending)
        {
            return sortBy.ToLower() switch
            {
                "username" => ascending 
                    ? query.OrderBy(u => u.Username) 
                    : query.OrderByDescending(u => u.Username),
                "firstname" => ascending 
                    ? query.OrderBy(u => u.FirstName) 
                    : query.OrderByDescending(u => u.FirstName),
                "lastname" => ascending 
                    ? query.OrderBy(u => u.LastName) 
                    : query.OrderByDescending(u => u.LastName),
                "email" => ascending 
                    ? query.OrderBy(u => u.Email) 
                    : query.OrderByDescending(u => u.Email),
                "createdat" => ascending 
                    ? query.OrderBy(u => u.CreatedAt) 
                    : query.OrderByDescending(u => u.CreatedAt),
                _ => ascending 
                    ? query.OrderBy(u => u.Username) 
                    : query.OrderByDescending(u => u.Username)
            };
        }

        private async Task InvalidateUserCacheAsync(int? userId = null)
        {
            _logger.LogInfo("Kullanıcı önbelleği temizleniyor.");
            
            // Tüm kullanıcılar önbelleğini temizle
            await _cacheService.RemoveAsync(CACHE_KEY_ALL_USERS);
            
            // Kullanıcı özetleri önbelleğini temizle
            await _cacheService.RemoveAsync(CACHE_KEY_USER_SUMMARIES);
            
            // Sayfalanmış kullanıcılar önbelleğini temizle (tüm sayfalar)
            // Not: Daha gelişmiş bir yaklaşım, önbellekteki tüm sayfalanmış kullanıcı anahtarlarını izlemek olabilir
            
            // Belirli bir kullanıcı ID'si verilmişse, o kullanıcıya özgü önbelleği temizle
            if (userId.HasValue)
            {
                await _cacheService.RemoveAsync($"{CACHE_KEY_USER_BY_ID}{userId.Value}");
            }
        }

        public async Task<User> GetByIdWithRoleAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllWithRolesAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => 
                    u.Username.Contains(searchTerm) || 
                    u.Email.Contains(searchTerm) || 
                    u.FullName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Include(u => u.Role)
                .OrderBy(u => u.Username)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }
    }
} 