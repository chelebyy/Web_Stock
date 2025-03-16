using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Kullanıcı repository'si
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Rol repository'si
        /// </summary>
        IRoleRepository Roles { get; }

        /// <summary>
        /// İzin repository'si
        /// </summary>
        IPermissionRepository Permissions { get; }

        /// <summary>
        /// Ürün repository'si
        /// </summary>
        IProductRepository Products { get; }

        /// <summary>
        /// Kategori repository'si
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// Repository erişimi
        /// </summary>
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Değişiklikleri kaydeder
        /// </summary>
        /// <returns>Etkilenen satır sayısı</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Yeni bir transaction başlatır
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Transaction'ı commit eder
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Transaction'ı rollback eder
        /// </summary>
        Task RollbackTransactionAsync();
    }
} 