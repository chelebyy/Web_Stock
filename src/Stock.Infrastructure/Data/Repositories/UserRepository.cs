using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Kullanıcı (User) entity'si için repository sınıfı.
    /// GenericRepository<User> sınıfından türetilmiştir ve IUserRepository arayüzünü implemente eder.
    /// Spesifik sorgular için Specification Pattern kullanılır.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// UserRepository sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı context'i.</param>
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            // Base class constructor'ı çağrılır.
            // Spesifik metotlar kaldırıldığı için burada ek bir implementasyon gerekmez.
        }

        public async Task<User?> GetBySicilAsync(string sicil)
        {
            return await _dbSet
                .Where(u => u.Sicil == sicil && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}