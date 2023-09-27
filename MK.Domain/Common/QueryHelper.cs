using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Common
{
    public class QueryHelper<T> where T : class
    {
        public PaginationParameters? PaginationParams { get; set; } = null;
        /// <summary>
        /// Helper for select field from entity - SELECT command
        /// </summary>
        public Expression<Func<T, T>> Selector { get; set; } = null!;
        /// <summary>
        /// Helper for filter entity with condition - WHERE command
        /// </summary>
        public Expression<Func<T, bool>>? Filter { get; set; } = null;
        /// <summary>
        /// Helper for include entity - JOIN command
        /// </summary>
        public Expression<Func<T, object>>[]? Includes { get; set; } = null;
    }
}
