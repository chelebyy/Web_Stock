using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Rol (Role) entity'si için repository arayüzü.
    /// Temel CRUD işlemleri IRepository<Role> arayüzünden gelir.
    /// Role özel karmaşık sorgular gerekirse buraya eklenebilir.
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        // Spesifik metotlar kaldırıldı. Sorgular Specification Pattern ile yapılacak.
        // Örnek: GetByNameAsync yerine repository.FirstOrDefaultAsync(new RoleByNameSpecification(name))
    }
} 