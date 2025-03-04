using System;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 