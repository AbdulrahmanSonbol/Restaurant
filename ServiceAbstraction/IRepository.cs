using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ServiceAbstraction
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {

        // read 
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        Task<TEntity?> GetEntityWithSpecAsync(ISpecifications<TEntity, TKey> spec, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<IReadOnlyList<TEntity>> ListWithSpecAsync(ISpecifications<TEntity, TKey> spec, CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecifications<TEntity, TKey> spec, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<TEntity , bool>> predicate, CancellationToken ct = default);


        // write
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void UpdateRange(ICollection<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(ICollection<TEntity> entities);

    }
}
