using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Rol (Role) entity'si için repository sınıfı.
    /// GenericRepository<Role> sınıfından türetilmiştir ve IRoleRepository arayüzünü implemente eder.
    /// Spesifik sorgular için Specification Pattern kullanılır.
    /// </summary>
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        /// <summary>
        /// RoleRepository sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı context'i.</param>
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            // Base class constructor'ı çağrılır.
        }

        // Override edilen GetByIdAsync ve GetAllAsync metotları kaldırıldı.
        // GetByNameAsync ve GetRolesWithUsersAsync metotları kaldırıldı.
        // Bu işlevler artık ilgili Specification sınıfları ve base repository metotları ile sağlanır.
        // Örnek kullanım (Service/Handler katmanında):
        // var role = await _roleRepository.FirstOrDefaultAsync(new RoleByNameSpecification(name));
        // var rolesWithUsers = await _roleRepository.ListAsync(new RolesWithUsersSpecification());
    }
} 