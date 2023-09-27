using LinqKit;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MK.Infrastructure.Common
{
    public static class RepositoryHelpers
    {
        /// <summary>
        /// Excute include for a list of include expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<T> Includes<T>(this IQueryable<T> query, params Expression<Func<T, object>>[]? includes) where T : BaseEntity
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }
        /// <summary>
        /// Add where condition for query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<T> WhereCondition<T>(this IQueryable<T> query, Expression<Func<T, bool>>? whereCondition) where T : BaseEntity
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (whereCondition != null)
            {
                query.Where(whereCondition);
            }

            return query;
        }
        /// <summary>
        /// Add select field for query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<T> SelectField<T>(this IQueryable<T> query, Expression<Func<T, T>> selector) where T : BaseEntity
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (selector != null)
            {
                query.Select(selector);
            }

            return query;
        }
        /// <summary>
        /// Add condition IsDeleted = false for query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> AddExistCondition<T>(this Expression<Func<T, bool>>? filter) where T : BaseEntity
        {
            Expression<Func<T, bool>> isNotDeleteCondition = p => p.IsDeleted == false;

            if (filter == null)
            {
                filter = isNotDeleteCondition;
            }
            else
            {
                filter = PredicateBuilder.And(isNotDeleteCondition, filter);
            }

            return filter;
        }
        /// <summary>
        /// Apply conditions for query helper (AsNoTracking)
        /// AddExistCondition -> Includes -> WhereCondition -> SelectField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyConditions<T>(this DbSet<T> dbSet, QueryHelper<T> queryHelper, Guid? id = null, bool isAsNoTracking = true) where T : BaseEntity
        {
            Expression<Func<T, bool>> isNotDeleteCondition = p => p.Id == id;

            queryHelper.Filter = queryHelper.Filter.AddExistCondition();

            if (id != null)
            {
                queryHelper.Filter = PredicateBuilder.And(queryHelper.Filter, isNotDeleteCondition);
            }

            IQueryable<T> query = dbSet;

            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }

            query.Includes(queryHelper.Includes)
            .WhereCondition(queryHelper.Filter)
            .SelectField(queryHelper.Selector);

            return query;
        }
    }
}
