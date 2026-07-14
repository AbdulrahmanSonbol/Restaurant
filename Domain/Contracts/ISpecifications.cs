using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> Include { get; }

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public Expression<Func<TEntity, object>> OrderBy { get; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; }

        public int Take { get; }

        public int Skip { get; }

        public bool IsPaginated { get; }

        public bool IsAsNoTracking { get; }

    }
}