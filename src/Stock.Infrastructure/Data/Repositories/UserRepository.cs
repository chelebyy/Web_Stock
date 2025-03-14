using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Linq;

namespace Stock.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.Role)
                .ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetBySicilAsync(string sicil)
        {
            return await _dbSet
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Sicil == sicil);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await _dbSet
                .Include(x => x.Role)
                .ToListAsync();
        }
        
        // Sayfalama için yeni metot
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _dbSet
                .CountAsync();
                
            var users = await _dbSet
                .Include(x => x.Role)
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (users, totalCount);
        }
        
        // Projection için yeni metot
        public async Task<IEnumerable<User>> GetUserSummariesAsync()
        {
            return await _dbSet
                .Include(x => x.Role)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Sicil = u.Sicil,
                    IsAdmin = u.IsAdmin,
                    Role = new Role { Name = u.Role.Name }
                })
                .ToListAsync();
        }
    }
} 