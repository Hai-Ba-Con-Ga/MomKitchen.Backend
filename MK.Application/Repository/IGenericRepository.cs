
namespace MK.API.Application.Repository
{
    public interface IGenericRepository<T> : IDisposable where T : class, new()
    {
        Task CreateAsync(T entity);
        Task<int> DeleteAsync(Func<T, bool> filter);
        Task<int> SoftDeleteAsync(Func<T, bool> filter);
        void Update(T entity);
        Task<T> GetById(Guid id, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams);
        Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams, Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
