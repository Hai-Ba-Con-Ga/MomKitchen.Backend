using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MK.Infrastructure.Common
{
    /// <summary>
    /// There are some helper methods for generic repository 
    /// </summary>
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
                query = query.Where(whereCondition);
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
        public static IQueryable<TResult> SelectField<TSource, TResult>(this IQueryable<TSource> query, Expression<Func<TSource, TResult>> selector) where TSource : BaseEntity where TResult : class
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (selector == null)
                return query.Select(CreateProjection<TSource, TResult>());

            return query.Select(selector);
        }
        /// <summary>
        /// Add select field for query by selected selectedFields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="selectedFields"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IQueryable<T> SelectField<T>(this IQueryable<T> query, string[] selectedFields) where T : BaseEntity
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (selectedFields != null && selectedFields.Length > 0)
            {
                query = query.Select(CreateProjection<T>(selectedFields));
            }

            return query;
        }
        /// <summary>
        /// Add condition IsDeleted = false for query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AddExistCondition<T>(this Expression<Func<T, bool>>? filter) where T : BaseEntity
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
        /// Require: QueryHelper<TSource> queryHelper
        /// 
        /// Apply conditions for query helper (AsNoTracking) 
        /// 
        /// AddExistCondition -> Includes -> WhereCondition -> SelectField
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ApplyConditions<TSource>(this DbSet<TSource> dbSet, QueryHelper<TSource> queryHelper, Guid? id = null, bool isAsNoTracking = true) where TSource : BaseEntity
        {
            Expression<Func<TSource, bool>> matchWithId = p => p.Id == id;

            queryHelper.Filter = queryHelper.Filter.AddExistCondition();

            if (id != null)
            {
                queryHelper.Filter = PredicateBuilder.And(queryHelper.Filter, matchWithId);
            }

            IQueryable<TSource> query = dbSet;

            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.Includes(queryHelper.Includes)
                     .WhereCondition(queryHelper.Filter)
                     .SelectField(queryHelper.SelectedFields);
        }
        /// <summary>
        /// Require: QueryHelper<TSource, TResult> queryHelper
        /// 
        /// Apply conditions for query helper (AsNoTracking)
        /// 
        /// AddExistCondition -> Includes -> WhereCondition -> SelectField
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        public static IQueryable<TResult> ApplyConditions<TSource, TResult>(this DbSet<TSource> dbSet, QueryHelper<TSource, TResult> queryHelper, Guid? id = null, bool isAsNoTracking = true) where TSource : BaseEntity where TResult : class
        {
            queryHelper.Filter = queryHelper.Filter.AddExistCondition();

            if (id != null)
            {
                queryHelper.Filter = PredicateBuilder.And(queryHelper.Filter, t => t.Id.Equals(id));
            }

            IQueryable<TSource> query = dbSet;

            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.Includes(queryHelper.Includes)
                        .WhereCondition(queryHelper.Filter)
                        .SelectField<TSource, TResult>(queryHelper.Selector);
        }

        #region Dynamic query extension
        /// <summary>
        /// Function create expression for select field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedFields"></param>
        /// <returns>Expression of enity, but just get fields in selectedFields</returns>
        static Expression<Func<T, T>> CreateProjection<T>(string[] selectedFields)
        {
            return entity => CreateProjectedInstance(entity, selectedFields);
        }
        /// <summary>
        /// Function create expression for select field
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>Expression of mapping between TSource and TResult (Enity - DTO)</returns>
        static Expression<Func<TSource, TResult>> CreateProjection<TSource, TResult>() where TSource : BaseEntity where TResult : class
        {
            var properties = typeof(TResult).GetProperties();
            return entity => CreateProjectedInstance<TSource, TResult>(entity, properties);
        }

        static TResult CreateProjectedInstance<TSource, TResult>(TSource source, PropertyInfo[]? resultProperties)
        {
            if (resultProperties == null)
            {
                throw new ArgumentNullException(nameof(resultProperties));
            }

            var resultInstance = Activator.CreateInstance<TResult>();

            var sourceProperties = typeof(TSource).GetProperties();

            foreach (var resultProperty in resultProperties)
            {
                if (resultProperty == null)
                {
                    continue;
                }

                var sourceProperty = sourceProperties.FirstOrDefault(p => p.Name == resultProperty.Name);

                if (sourceProperty != null)
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    resultProperty.SetValue(resultInstance, sourceValue);
                }
            }

            return resultInstance;
        }

        static T CreateProjectedInstance<T>(T source, string[] selectedFields)
        {
            var projectedInstance = Activator.CreateInstance<T>();

            var properties = typeof(T).GetProperties();

            foreach (var field in selectedFields)
            {
                var property = properties.FirstOrDefault(p => p.Name == field);

                if (property != null)
                {
                    var value = property.GetValue(source);
                    property.SetValue(projectedInstance, value);
                }
            }

            return projectedInstance;
        }
        #endregion Dynamic query extension

        #region Update extension
        public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> GetSetPropertyCalls<TEntity, TDto>(this TDto dto) where TEntity : BaseEntity where TDto : class
        {
            var dtoProperties = typeof(TDto).GetProperties();
            var entityProperties = typeof(TEntity).GetProperties();

            //Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter = calls => calls;


            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty is null)
                {
                    continue;
                }

                var nameProperty = entityProperties.FirstOrDefault(p => p.Name == dtoProperty.Name)?.Name;

                if (nameProperty is not null)
                {
                    Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> expression = setter => setter.SetProperty(t => t.GetType().GetProperty(nameProperty), dto.GetType().GetProperty(nameProperty));
                    return expression;
                }
            }

            return null;
        }

        public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> AppendSetProperty<TEntity>(
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> left,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> right)
        {
            var replace = new ReplacingExpressionVisitor(right.Parameters, new[] { left.Body });
            var combined = replace.Visit(right.Body);
            return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(combined, left.Parameters);
        }

        #endregion Update extension
    }
}
