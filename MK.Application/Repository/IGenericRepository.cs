

namespace MK.API.Application.Repository
{
    public interface IGenericRepository<T> : IDisposable where T : BaseEntity, new()
    {
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Create an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<Guid> CreateAsync(T entity, bool isSaveChange = false);

        /// <summary>
        /// Create multiple entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<IEnumerable<Guid>> CreateAsync(IEnumerable<T> entities, bool isSaveChange = false);

        /// <summary>
        /// Hard delete
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Update IsDeleted = true
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Update traditional
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, bool isSaveChange = false);

        /// <summary>
        /// Update without SaveChanges
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="setPropertyCalls"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);

        /// <summary>
        /// Get an entity is active and match condition predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        #region Version 2.0
        /// <summary>
        /// GetAll entity by id and other conditions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<T?> GetById(Guid id, QueryHelper<T> query = null, bool isAsNoTracking = true);

        /// <summary>
        /// GetAll entity by id and other conditions
        /// This function will return mapping dto object map from entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns>
        /// Null - if entity not found
        /// </returns>
        Task<TResult?> GetById<TResult>(Guid id, QueryHelper<T, TResult> queryHelper = null, bool isAsNoTracking = true) where TResult : class;

        /// <summary>
        /// Get an entity is active and match condition predicate
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <param name="isAsNoTracking"></param>
        /// <returns></returns>
        Task<T?> GetFirstOrDefault(QueryHelper<T> queryHelper, bool isAsNoTracking = true);

        /// <summary>
        /// Get an entity has mapping dto object is active and match condition predicate
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryHelper"></param>
        /// <param name="isAsNoTracking"></param>
        /// <returns></returns>
        Task<TResult?> GetFirstOrDefault<TResult>(QueryHelper<T, TResult> queryHelper, bool isAsNoTracking = true) where TResult : class;

        /// GetAll all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Get(QueryHelper<T> queryHelper = null, bool isAsNoTracking = true);

        /// <summary>
        /// GetAll all entities are active and match condition predicate, this function is AsNoTracking
        /// 
        /// This function will return list of mapping dto object map from list of entity
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> Get<TResult>(QueryHelper<T, TResult> queryHelper = null, bool isAsNoTracking = true) where TResult : class;

        /// <summary>
        /// GetAll all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<T> and it is used to represent pagination of a list of objects.
        /// </returns>
        Task<PagedList<T>> GetWithPagination(QueryHelper<T> queryHelper = null, bool isAsNoTracking = true);

        /// <summary>
        /// GetAll all entities are active and match condition predicate, this function is AsNoTracking
        /// 
        /// This function will return PagedList of mapping dto object map from list of entity
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<TSource> and it is used to represent pagination of a list of objects.
        /// </returns>
        Task<PagedList<TResult>> GetWithPagination<TResult>(QueryHelper<T, TResult> queryHelper = null, bool isAsNoTracking = true) where TResult : class;

        /// <summary>
        /// Check entity exist in database by condition predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> IsExist(Expression<Func<T, bool>> predicate);

        #endregion Version 2.0

        #region Function had been removed in version 2.0
        //Task<T?> GetById(Guid id, Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        //Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, T>> selector, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        #endregion 

        #region Comming soon  - Địt chưa làm đc đừng có sài

        /// <summary>
        /// Update entity by id and other conditions with DTO object - Hàng lỗi nha đừng có sài 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<int> UpdateAsync<TDto>(Expression<Func<T, bool>> predicate, TDto req) where TDto : class, new();

        #endregion Comming soon 
    }
}
