
namespace MK.Infrastructure.Common
{
    public static class LinqExtension
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }
    }
}
