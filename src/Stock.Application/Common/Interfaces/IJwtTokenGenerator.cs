using Stock.Domain.Entities;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// JWT (JSON Web Token) oluşturma işlemlerini tanımlayan arayüz.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Belirtilen kullanıcı bilgileri için asenkron olarak bir JWT oluşturur.
        /// </summary>
        /// <param name="user">Token oluşturulacak kullanıcı nesnesi.</param>
        /// <returns>Oluşturulan JWT token string'ini içeren bir Task.</returns>
        Task<string> GenerateTokenAsync(User user);

        /// <summary>
        /// Belirtilen kullanıcı bilgileri için senkron olarak bir JWT oluşturur.
        /// </summary>
        /// <param name="user">Token oluşturulacak kullanıcı nesnesi.</param>
        /// <returns>Oluşturulan JWT token string'i.</returns>
        string GenerateToken(User user);
    }
} 