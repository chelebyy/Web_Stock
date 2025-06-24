using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Infrastructure.Data.Specifications;

namespace Stock.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Belirli bir entity türü için temel veri erişim işlemlerini sağlayan genel repository sınıfı.
    /// Güncellenmiş IRepository<T> arayüzünü implemente eder ve Specification deseni kullanır.
    /// </summary>
    /// <typeparam name="T">Repository'nin yöneteceği entity türü (BaseEntity'den türemelidir).</typeparam>
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Veritabanı context nesnesi.
        /// </summary>
        protected readonly ApplicationDbContext _context;
        /// <summary>
        /// Bu repository tarafından yönetilen entity için DbSet nesnesi.
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// GenericRepository sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı context'i.</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <inheritdoc/>
        public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            // Eğer specification açıkça AsNoTracking olarak işaretlenmişse, sorguya uygulanmış olacaktır
            // Değilse, sorguya AsNoTracking uygulamıyoruz çünkü birçok durumda FirstOrDefault sonucunun 
            // güncellenmesi veya izlenmesi gerekebilir
            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            // Performans optimizasyonu: Liste sorgularında varsayılan olarak AsNoTracking kullan
            // Specification zaten AsNoTracking'i açıkça belirtmiyorsa
            if (!spec.IsAsNoTracking)
            {
                var query = ApplySpecification(spec).AsNoTracking();
                return await query.ToListAsync(cancellationToken);
            }
            
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Primary key üzerinden arama yapmak için FindAsync en verimli yöntemdir.
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<T>> ListEnumerableAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            // Performans optimizasyonu: Liste sorgularında varsayılan olarak AsNoTracking kullan
            // Specification zaten AsNoTracking'i açıkça belirtmiyorsa
            if (!spec.IsAsNoTracking)
            {
                var query = ApplySpecification(spec).AsNoTracking();
                return await query.ToListAsync(cancellationToken);
            }
            
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            // Count sorguları için değişiklik izlemeye gerek yoktur, bu yüzden AsNoTracking kullanmalıyız
            var query = ApplySpecification(spec, evaluateCriteriaOnly: true).AsNoTracking();
            return await query.CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            // EF Core AddAsync doesn't directly return the tracked entity in the same way some patterns might expect.
            // We return the input entity as it is now tracked by the context.
            // For AddAsync, EF Core tracks the entity, and its state will be 'Added'.
            // The actual ID generation (if applicable) happens upon SaveChangesAsync.
            // Returning the entity allows chaining or further inspection if needed.
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Returns all entities that are not marked as deleted.
            return await _dbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual void Update(T entity)
        {
            // Mark the entity as modified. This is the synchronous version required by the interface.
             if (entity == null)
             {
                 throw new ArgumentNullException(nameof(entity), "Entity to update cannot be null.");
             }
            _context.Entry(entity).State = EntityState.Modified;
        }
        
        /// <inheritdoc/>
        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Asynchronous wrapper for Update.
            Update(entity);
            // Return a completed task as the operation is synchronous in terms of EF state change.
            // The actual database update happens during SaveChangesAsync.
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual void Delete(T entity)
        {
             // Mark the entity for removal. This is the synchronous version required by the interface.
             if (entity == null)
             {
                 throw new ArgumentNullException(nameof(entity), "Entity to delete cannot be null.");
             }
            _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Asynchronous wrapper for Delete.
            Delete(entity);
            // Return a completed task as the operation is synchronous in terms of EF state change.
            // The actual database removal happens during SaveChangesAsync.
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <inheritdoc/>
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Persist changes to the database.
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Verilen specification'ı DbSet üzerine uygular.
        /// </summary>
        /// <param name="spec">Uygulanacak specification.</param>
        /// <param name="evaluateCriteriaOnly">Sadece filtreleme kriterlerini mi uygulayacak (CountAsync gibi durumlar için)?</param>
        /// <returns>Specification uygulanmış IQueryable<T> nesnesi.</returns>
        protected IQueryable<T> ApplySpecification(ISpecification<T> spec, bool evaluateCriteriaOnly = false)
        {
             // evaluateCriteriaOnly parametresi SpecificationEvaluator'da henüz desteklenmiyor.
             // CountAsync için daha optimize bir yol ileride eklenebilir.
            return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
        }

        // Eksik metot eklendi
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
             // IsDeleted filtresini burada da uygulamak önemli!
             return await _dbSet.Where(e => !e.IsDeleted).AnyAsync(predicate, cancellationToken);
        }
    }
} 