
namespace MK.API.Application.Repository
{
    public interface IGenericRepository<T> : IDisposable where T : BaseEntity, new()
    {
        Task<int> SaveChangesAsync();
     
        Task<Guid> CreateAsync(T entity, bool isSaveChange = false);
        Task<IEnumerable<Guid>> CreateAsync(IEnumerable<T> entities, bool isSaveChange = false);

        Task<int> DeleteAsync(Expression<Func<T, bool>> filter);
        Task<int> SoftDeleteAsync(Expression<Func<T, bool>> filter);

        void Update(T entity, bool isSaveChange = false);
        Task<int> Update(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);

        Task<T?> GetById(Guid id, Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);

        #region Version 2.0
        /// <summary>
        /// Get entity by id and other conditions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<T?> GetById(Guid id, QueryHelper<T> query, bool isAsNoTracking = true);
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Get(QueryHelper<T> queryHelper, bool isAsNoTracking = true);
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<T> and it is used to represent pagination of a list of objects.
        /// </returns>
        Task<PagedList<T>> GetWithPagination(QueryHelper<T> queryHelper, bool isAsNoTracking = true);
        #endregion Version 2.0
    }
}
