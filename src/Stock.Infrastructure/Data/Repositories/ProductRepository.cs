using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Data.Specifications;
using System.Threading; // CancellationToken için eklendi

namespace Stock.Infrastructure.Data.Repositories
{
    // GenericRepository<Product>'tan miras al ve IProductRepository'yi implemente et
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // _context ve tekrarlanan metotlar kaldırıldı. Constructor base'e context'i iletiyor.
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        // GetByIdAsync, GetAllAsync, AddAsync, UpdateAsync GenericRepository'den geliyor, kaldırıldı.

        // DeleteAsync override edilerek soft delete uygulandı
        public override Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Entity to delete cannot be null.");
            }
            product.IsDeleted = true; // Soft delete
            _context.Entry(product).State = EntityState.Modified;
            // SaveChangesAsync UnitOfWork tarafından çağrılacak.
            return Task.CompletedTask;
        }

        // GetAsync metodu GenericRepository'deki ListAsync ile aynı işlevi görüyor, kaldırıldı.
        // SaveChangesAsync metodu kaldırıldı, UnitOfWork sorumluluğunda.

        // Product'a özel veritabanı işlemleri gerekirse buraya eklenebilir.
        // Örneğin:
        // public async Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName)
        // {
        //     // Specification kullanarak yapmak daha uygun olabilir:
        //     var spec = new ProductsByCategoryNameSpecification(categoryName);
        //     return await ListAsync(spec);
        // }
    }

    // Örnek Specification (Gerekirse Domain katmanına taşınmalı)
    // public class ProductsByCategoryNameSpecification : BaseSpecification<Product>
    // {
    //     public ProductsByCategoryNameSpecification(string categoryName)
    //         : base(p => p.Category.Name == categoryName && !p.IsDeleted)
    //     {
    //         AddInclude(p => p.Category);
    //     }
    // }
} 