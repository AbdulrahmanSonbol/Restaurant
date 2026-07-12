
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.IdentityData.DBContexts;
using ServiceAbstraction;
using System.Linq.Expressions;
namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantIdentityDBContexts _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(RestaurantIdentityDBContexts context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}