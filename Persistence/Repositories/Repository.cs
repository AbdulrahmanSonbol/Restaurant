
using Microsoft.EntityFrameworkCore;
using Persistence.IdentityData.DBContexts;
using ServiceAbstraction;
using System.Linq.Expressions;
namespace Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantIdentityDBContexts _context;
        private readonly DbSet<T> _dbSet;

        public Repository(RestaurantIdentityDBContexts context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void DeleteRange(ICollection<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return _dbSet.AnyAsync(predicate, ct);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            var res = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return res;
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public IQueryable<T> GetTableAsTracking()
        {
            return _dbSet.AsTracking().AsQueryable();
        }

        public IQueryable<T> GetTableNoTracking()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(ICollection<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }

}
