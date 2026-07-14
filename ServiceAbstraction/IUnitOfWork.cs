using System;

namespace  ServiceAbstraction 
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
     
        Task CommitAsync(CancellationToken cancellationToken = default);
        
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
