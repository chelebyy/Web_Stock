using System;
using System.Threading.Tasks;
using Stock.Domain.Entities;
using System.Threading;

namespace Stock.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IActivityLogRepository ActivityLogs { get; }
        
        /// <summary>
        /// Belirtilen entity türü için Generic Repository örneği döndürür.
        /// </summary>
        /// <typeparam name="T">Repository'nin yöneteceği BaseEntity'den türemiş entity türü.</typeparam>
        /// <returns>Belirtilen tür için IRepository<T> örneği.</returns>
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        
        /// <summary>
        /// Yapılan tüm değişiklikleri veritabanına kaydeder.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Etkilenen satır sayısı.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Yeni bir veritabanı transaction'ı başlatır.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Mevcut transaction'ı commit eder.
        /// Hata oluşursa otomatik olarak rollback yapar.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Mevcut transaction'ı rollback yapar.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
} 