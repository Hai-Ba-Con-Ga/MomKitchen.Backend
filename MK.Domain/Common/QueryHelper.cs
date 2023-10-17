using Microsoft.EntityFrameworkCore.Query;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Common
{
    /// <summary>
    /// This is a helper class for query, it's properties will be used for query and these are optional
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryHelper<T> where T : BaseEntity
    {
        /// <summary>
        /// Helper for pagination - LIMIT, OFFSET command
        /// </summary>
        public PaginationParameters? PaginationParams { get; set; } = null;
        /// <summary>
        /// Helper for filter entity with condition - WHERE command
        /// </summary>
        public Expression<Func<T, bool>>? Filter { get; set; } = null;
        /// <summary>
        /// Helper for include entity - JOIN command
        /// </summary>
        //public Expression<Func<T, object>>[]? Includes { get; set; } = null;
        /// <summary>
        /// Helper for include entity - JOIN command
        /// </summary>
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Include = null;
        /// <summary>
        /// Helper for select field from entity - SELECT command
        /// </summary>
        public Expression<Func<T, T>> Selector { get; set; } = null!;
        /// <summary>
        /// Helper for select field from entity - SELECT command
        /// </summary>
        public string[] SelectedFields { get; set; } = null!;
    }
    /// <summary>
    /// QueryHelper<TSource, TResult> TSource is an entity, TResult is a dto object
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class QueryHelper<TSource, TResult> : QueryHelper<TSource> where TSource : BaseEntity where TResult : class
    {
        /// <summary>
        /// Helper for select field from entity - SELECT command
        /// </summary>
        public new Expression<Func<TSource, TResult>> Selector { get; set; } = null!;
    }

}
