using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Role?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.Users)
                .ToListAsync();
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet
                .Include(x => x.Users)
                .ToListAsync();
        }
    }
} 