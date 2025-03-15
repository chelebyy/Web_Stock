using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetRolesWithUsersAsync();
        
        /// <summary>
        /// Sayfalama ve filtreleme ile rolleri getirme
        /// </summary>
        Task<(IEnumerable<Role> Roles, int TotalCount)> GetPaginatedRolesAsync(
            int pageNumber, 
            int pageSize, 
            string nameFilter = null,
            bool includeUsers = false,
            string sortBy = "Name",
            bool sortAscending = true);
    }
} 