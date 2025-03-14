using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;
using Stock.Application.Models.DTOs;

namespace Stock.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetBySicilAsync(string sicil);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
        
        // Sayfalama için yeni metot
        Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        
        // Projection için yeni metot
        Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync();
    }
} 