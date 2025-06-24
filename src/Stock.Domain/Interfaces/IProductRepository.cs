using Stock.Domain.Entities;
// using Stock.Domain.Specifications; // IRepository<T>'de zaten tanımlı, burada gerek yok

namespace Stock.Domain.Interfaces
{
    // IRepository<Product>'tan miras al
    public interface IProductRepository : IRepository<Product>
    {
        // GetByIdAsync, GetAllAsync, AddAsync, UpdateAsync, DeleteAsync gibi temel metotlar
        // IRepository<Product>'tan geldiği için burada tekrar tanımlamaya gerek yok.

        // Sadece Product'a özel ek metotlar (varsa) buraya tanımlanır.
        // Örneğin:
        // Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName);

        // Mevcut GetAsync(ISpecification<Product> spec) metodu da IRepository<T>'de
        // ListAsync(ISpecification<T> spec) olarak tanımlı olduğu için kaldırıldı.
    }
} 