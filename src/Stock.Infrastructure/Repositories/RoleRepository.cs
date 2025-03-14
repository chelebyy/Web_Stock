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

        public virtual async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public virtual async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet
                .Include(r => r.Users)
                .ToListAsync();
        }
        
        public override async Task<Role?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        
        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }
        
        public override async Task AddAsync(Role entity)
        {
            await base.AddAsync(entity);
        }
        
        public override async Task UpdateAsync(Role entity)
        {
            await base.UpdateAsync(entity);
        }
        
        public override async Task DeleteAsync(Role entity)
        {
            await base.DeleteAsync(entity);
        }
    }
} 