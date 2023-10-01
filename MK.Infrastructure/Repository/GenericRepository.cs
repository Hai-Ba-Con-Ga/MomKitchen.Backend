

using Mapster;
using MK.Domain.Common;
using MK.Infrastructure.Common;
using System;
using System.Linq.Expressions;

namespace MK.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext dbContext;
        protected DbSet<T> dbSet;
        public GenericRepository(DbContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        #region Create
        /// <summary>
        /// Add an entity to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Guid> CreateAsync(T entity, bool isSaveChange = false)
        {
            await dbSet.AddAsync(entity);
            if (isSaveChange)
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
            return entity.Id;
        }
        /// <summary>
        /// Add a list of entities to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Guid>> CreateAsync(IEnumerable<T> entities, bool isSaveChange = false)
        {
            await dbSet.AddRangeAsync(entities);
            if (isSaveChange)
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
            return entities.Select(e => e.Id);
        }
        #endregion Create

        #region Delete
        /// <summary>
        /// Delete immediately by condition predicate without SaveChanges action 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate)
                                .ExecuteDeleteAsync()
                                .ConfigureAwait(false);
        }
        /// <summary>
        /// UpdateAsync IsDeleted to true by condition predicate without SaveChanges action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate)
                                .ExecuteUpdateAsync(setter => setter.SetProperty(e => e.IsDeleted, true))
                                .ConfigureAwait(false);
        }
        #endregion Delete

        #region Update
        /// <summary>
        ///  Change stated of entity to Modified (mark this entity will update), need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entity"></param>
        public async Task<int> UpdateAsync(T entity, bool isSaveChange = false)
        {
            dbContext.Attach(entity).State = EntityState.Modified;
            if (isSaveChange)
            {
                return await SaveChangesAsync();
            }
            return 0;
        }
        /// <summary>
        /// Update entity exist in database by condition predicate without SaveChanges action
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="setPropertyCalls"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            var query = dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ExecuteUpdateAsync(setPropertyCalls);
        }
        /// <summary>
        /// Update entity by id and other conditions with DTO object
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> UpdateAsync<TDto>(Expression<Func<T, bool>> predicate, TDto req) where TDto : class, new()
        {
            var setPropertyCalls = req.GetSetPropertyCalls<T, TDto>();

            if (setPropertyCalls == null)
            {
                throw new ArgumentNullException("UpdateAsync - SetPropertyCalls is null");
            }

            return await UpdateAsync(predicate, setPropertyCalls);
        }
        #endregion Update

        #region Retrieve
        /// <summary>
        /// Get an entity is active by id and match orther condition predicate, this function is AsNoTracking 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate">can null</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<T?> GetById(Guid id, Expression<Func<T, T>> selector, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes)
        {
            Expression<Func<T, bool>> isNotDeleteCondition = p => p.IsDeleted == false && p.Id == id;

            if (predicate == null)
            {
                predicate = isNotDeleteCondition;
            }
            else
            {
                predicate = PredicateBuilder.And(isNotDeleteCondition, predicate);
            }

            if (includes == null)
            {
                return await dbSet.AsNoTracking().Where(predicate).SingleOrDefaultAsync();
            }
            else
            {
                var query = dbSet.AsNoTracking().Where(predicate);
                return await query
                            .Includes(includes)
                            .Select(selector)
                            .SingleOrDefaultAsync();
            }
        }

        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, T>> selector, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes)
        {
            Expression<Func<T, bool>> isNotDeleteCondition = p => p.IsDeleted == false;

            if (predicate == null)
            {
                predicate = isNotDeleteCondition;
            }
            else
            {
                predicate = PredicateBuilder.And(isNotDeleteCondition, predicate);
            }

            if (includes == null)
            {
                return await dbSet.AsNoTracking().Where(predicate).ToListAsync();
            }
            else
            {
                var query = dbSet.AsNoTracking().Where(predicate);

                return query.Includes(includes)
                            .Select(selector);
            }
        }

        // get first or default
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return dbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        #endregion Retrieve

        /// <summary>
        /// Function save changes to database (Excute command to Db like: update, create, delete)
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        #region Retrieve Version 2.0
        /// <summary>
        /// Get entity by id and other conditions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<T?> GetById(Guid id, QueryHelper<T> queryHelper, bool isAsNoTracking = true)
        {
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T>();
            }
            var query = dbSet.ApplyConditions(queryHelper, id, isAsNoTracking);

            return await query.SingleOrDefaultAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get entity by id and other conditions
        /// 
        /// This function will return mapping dto object map from entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<TResult?> GetById<TResult>(Guid id, QueryHelper<T, TResult> queryHelper, bool isAsNoTracking = true) where TResult : class
        {
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T, TResult>();
            }
            var query = dbSet.ApplyConditions(queryHelper, id, isAsNoTracking);

            return await query.SingleOrDefaultAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Get(QueryHelper<T> queryHelper, bool isAsNoTracking = true)
        {
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T>();
            }
            var query = dbSet.ApplyConditions(queryHelper, isAsNoTracking: isAsNoTracking);

            return await query.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// 
        /// This function will return list of mapping dto object map from list of entity
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> Get<TResult>(QueryHelper<T, TResult> queryHelper, bool isAsNoTracking = true) where TResult : class
        {
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T, TResult>();
            }
            var query = dbSet.ApplyConditions(queryHelper, isAsNoTracking: isAsNoTracking);

            return await query.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<TSource> and it is used to represent pagination of a list of objects.
        /// </returns>
        public async Task<PagedList<T>> GetWithPagination(QueryHelper<T> queryHelper, bool isAsNoTracking = true)
        {
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T>();
            }
            var pagedList = new PagedList<T>();

            var query = dbSet.ApplyConditions(queryHelper, isAsNoTracking: isAsNoTracking);

            await pagedList.LoadData(query, queryHelper.PaginationParams).ConfigureAwait(false);

            return pagedList;
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// 
        /// This function will return PagedList of mapping dto object map from list of entity
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<TSource> and it is used to represent pagination of a list of objects.
        /// </returns>
        public async Task<PagedList<TResult>> GetWithPagination<TResult>(QueryHelper<T, TResult> queryHelper, bool isAsNoTracking = true) where TResult : class
        {
            var pagedList = new PagedList<TResult>();
            if (queryHelper == null)
            {
                queryHelper = new QueryHelper<T, TResult>();
            }

            var query = dbSet.ApplyConditions(queryHelper, isAsNoTracking: isAsNoTracking);

            await pagedList.LoadData(query, queryHelper.PaginationParams).ConfigureAwait(false);

            return pagedList;
        }
        #endregion Retrieve Version 2.0

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                dbSet = null;
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~GenericRepository()
        {
            Dispose(false);
        }
        #endregion Dispose
    }
}
