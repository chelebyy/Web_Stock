using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
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
                .Include(u => u.Role)
                .ToListAsync();
        }

        // Sayfalama için yeni metot
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _dbSet
                .CountAsync();
                
            var users = await _dbSet
                .Include(u => u.Role)
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (users, totalCount);
        }
        
        // Projection için yeni metot - domain katmanına uygun olarak güncellendi
        public async Task<IEnumerable<User>> GetUserSummariesAsync()
        {
            return await _dbSet
                .Include(u => u.Role)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Sicil = u.Sicil,
                    IsActive = u.IsActive,
                    Role = new Role { Name = u.Role.Name }
                })
                .ToListAsync();
        }
    }
} 