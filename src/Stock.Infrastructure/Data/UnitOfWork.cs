using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data.Repositories;
using System.Threading;

namespace Stock.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work deseni implementasyonu.
    /// Repository erişimini ve transaction yönetimini merkezileştirir.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private IPermissionRepository? _permissions;
        private IProductRepository? _products;
        private ICategoryRepository? _categories;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public IUserRepository Users => _users ??= new UserRepository(_context);
        /// <inheritdoc/>
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        /// <inheritdoc/>
        public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_context);
        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        
        /// <inheritdoc/>
        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            // TODO: Repository'leri cache'lemek veya DI ile yönetmek daha iyi olabilir.
            return new GenericRepository<T>(_context);
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Burada domain event'leri dispatch etme gibi ek mantıklar eklenebilir.
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken); // Commit öncesi SaveChanges çağrılmalı mı? Genellikle evet.
                await _context.Database.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken); // Rollback zaten CommitTransaction içinde yapılıyor gibi, ama yine de ekleyelim.
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            // Bekleyen değişiklikleri geri al (isteğe bağlı, context dispose edilince zaten kaybolur)
            // _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload()); 
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
} 