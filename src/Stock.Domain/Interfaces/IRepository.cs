using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Belirli bir entity türü için temel veri erişim işlemlerini tanımlayan genel repository arayüzü.
    /// Specification deseni kullanılarak sorgu esnekliği sağlanır.
    /// </summary>
    /// <typeparam name="T">Repository'nin yöneteceği entity türü (BaseEntity'den türemelidir).</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Belirtilen specification'a uyan ilk entity'yi veya varsayılan değeri asenkron olarak getirir.
        /// </summary>
        /// <param name="spec">Uygulanacak specification.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Specification'a uyan ilk entity veya null.</returns>
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Belirtilen specification'a uyan tüm entity'leri asenkron olarak getirir.
        /// </summary>
        /// <param name="spec">Uygulanacak specification.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Specification'a uyan entity'lerin listesi.</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Belirtilen ID'ye sahip entity'yi asenkron olarak getirir.
        /// </summary>
        /// <param name="id">Aranacak entity'nin ID'si.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>ID'ye sahip entity veya null.</returns>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Belirtilen specification'a uyan tüm entity'leri asenkron olarak getirir (IEnumerable olarak).
        /// </summary>
        /// <param name="spec">Uygulanacak specification.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Specification'a uyan entity'lerin IEnumerable koleksiyonu.</returns>
        Task<IEnumerable<T>> ListEnumerableAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Belirtilen specification'a uyan entity sayısını asenkron olarak getirir.
        /// </summary>
        /// <param name="spec">Uygulanacak specification.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Specification'a uyan entity sayısı.</returns>
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Belirtilen koşula uyan en az bir entity olup olmadığını asenkron olarak kontrol eder.
        /// </summary>
        /// <param name="predicate">Kontrol edilecek koşul.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Koşula uyan entity varsa true, yoksa false.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Yeni bir entity'yi veritabanına asenkron olarak ekler.
        /// </summary>
        /// <param name="entity">Eklenecek entity.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Birden fazla entity'yi veritabanına asenkron olarak ekler.
        /// </summary>
        /// <param name="entities">Eklenecek entity listesi.</param>
        /// <param name="cancellationToken">İptal token'ı.</param>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Mevcut bir entity'yi veritabanında günceller.
        /// Bu metot sadece entity'nin durumunu 'Modified' olarak işaretler, SaveChangesAsync çağrılmalıdır.
        /// </summary>
        /// <param name="entity">Güncellenecek entity.</param>
        void Update(T entity); // Genellikle senkron olabilir, context state'i değiştirir.

        /// <summary>
        /// Bir entity'yi veritabanından siler.
        /// Bu metot sadece entity'yi silinecek olarak işaretler, SaveChangesAsync çağrılmalıdır.
        /// </summary>
        /// <param name="entity">Silinecek entity.</param>
        void Delete(T entity); // Genellikle senkron olabilir, context state'i değiştirir.

        /// <summary>
        /// Birden fazla entity'yi veritabanından siler.
        /// </summary>
        /// <param name="entities">Silinecek entity listesi.</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Belirtilen specification'a uyan tüm entity'leri asenkron olarak getirir.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Specification'a uyan entity'lerin listesi.</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Mevcut bir entity'yi veritabanında günceller.
        /// Bu metot sadece entity'nin durumunu 'Modified' olarak işaretler, SaveChangesAsync çağrılmalıdır.
        /// </summary>
        /// <param name="entity">Güncellenecek entity.</param>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bir entity'yi veritabanından siler.
        /// Bu metot sadece entity'yi silinecek olarak işaretler, SaveChangesAsync çağrılmalıdır.
        /// </summary>
        /// <param name="entity">Silinecek entity.</param>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tüm değişiklikleri kaydeder.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Kaydedilen değişiklik sayısı.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 