using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Evaluator
{
    internal static class SpecificationsEvaluator // Create Query - Build Query
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>
            (IQueryable<TEntity> EntryPoint,
            ISpecifications<TEntity, TKey> specifications) where TEntity : class
        {
            var Query = EntryPoint;

            if (specifications is not null)
            {

                #region Criteria

                if (specifications.Criteria is not null)
                {
                    Query = Query.Where(specifications.Criteria);
                }

                #endregion

                #region Include

                if (specifications.IncludeExperessions is not null && specifications.IncludeExperessions.Any())
                {
                    foreach (var includeExp in specifications.IncludeExperessions)
                    {
                        Query = Query.Include(includeExp);
                    }
                }

                #endregion

                #region Sorting

                if (specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }

                else if (specifications.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDescending);
                }

                #endregion

                #region Pagination

                if (specifications.IsPaginated)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }

                #endregion

            }

            return Query;
        }
    }
}