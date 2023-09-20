
namespace MK.API.Application.Repository
{
    public interface IGenericRepository<T> : IDisposable where T : BaseEntity, new()
    {
        Task CreateAsync(T entity, bool isSaveChange = false);
        Task<int> DeleteAsync(Expression<Func<T, bool>> filter);
        Task<int> SoftDeleteAsync(Expression<Func<T, bool>> filter);
        void Update(T entity, bool isSaveChange = false);
        Task<int> Update(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
        Task<T?> GetById(Guid id, Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams);
        Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams, Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
