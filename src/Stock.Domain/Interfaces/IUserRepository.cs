using Stock.Domain.Entities;
using System.Threading.Tasks;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Kullanıcı (User) entity'si için repository arayüzü.
    /// Temel CRUD işlemleri IRepository<User> arayüzünden gelir.
    /// Kullanıcıya özel karmaşık sorgular gerekirse buraya eklenebilir.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        // Spesifik metotlar kaldırıldı. Sorgular Specification Pattern ile yapılacak.
        // Örnek: GetByUsernameAsync yerine repository.FirstOrDefaultAsync(new UserByUsernameSpecification(username))

        /// <summary>
        /// Verilen sicil numarasına sahip, silinmemiş kullanıcıyı getirir.
        /// </summary>
        /// <param name="sicil">Aranacak sicil numarası.</param>
        /// <returns>Eşleşen kullanıcı veya bulunamazsa null.</returns>
        Task<User?> GetBySicilAsync(string sicil);
    }
} 