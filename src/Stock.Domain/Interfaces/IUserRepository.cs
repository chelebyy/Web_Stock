using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetBySicilAsync(string sicil);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
    }
} 