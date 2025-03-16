using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdWithRoleAsync(int id);
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<User?> GetBySicilAsync(string sicil);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
        
        // Sayfalama ve filtreleme için geliştirilmiş metot
        Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(
            int pageNumber, 
            int pageSize, 
            string usernameFilter = null, 
            string sicilFilter = null, 
            int? roleIdFilter = null, 
            bool? isActiveFilter = null, 
            bool? isAdminFilter = null,
            string sortBy = "Username",
            bool sortAscending = true);
        
        // Projection için optimize edilmiş metot
        Task<IEnumerable<User>> GetUserSummariesAsync();
    }
} 