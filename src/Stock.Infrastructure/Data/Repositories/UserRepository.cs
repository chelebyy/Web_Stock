using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(int id)
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

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == username);
        }
    }
} 