
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.IdentityData.DBContexts;
using ServiceAbstraction;
using System.Linq.Expressions;
namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantIdentityDBContexts _dbContext;
    private readonly Dictionary<string, object> _repositories = new();
    private IDbContextTransaction? _transaction;

    public UnitOfWork(RestaurantIdentityDBContexts dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
    {
        var key = $"{typeof(TEntity).Name}_{typeof(TKey).Name}";

        if (!_repositories.ContainsKey(key))
        {
            var repositoryInstance = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(key, repositoryInstance);
        }

        return (IGenericRepository<TEntity, TKey>)_repositories[key];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _transaction?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}