using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ServiceAbstraction
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(ICollection<T> entities);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
        void UpdateRange(ICollection<T> entities);

    }
}
