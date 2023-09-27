

using MK.Domain.Common;
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
        public async Task CreateAsync(T entity, bool isSaveChange = false)
        {
            await dbSet.AddAsync(entity);
            if (isSaveChange)
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Add a list of entities to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task CreateAsync(IEnumerable<T> entities, bool isSaveChange = false)
        {
            await dbSet.AddRangeAsync(entities);
            if (isSaveChange)
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
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
        /// Update IsDeleted to true by condition predicate without SaveChanges action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate)
                                .ExecuteUpdateAsync(setter => setter.SetProperty(e => e.IsDeleted, false))
                                .ConfigureAwait(false);
        }
        #endregion Delete

        #region Update
        /// <summary>
        ///  Change stated of entity to Modified (mark this entity will update), need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entity"></param>
        public async void Update(T entity, bool isSaveChange = false)
        {
            dbContext.Attach(entity).State = EntityState.Modified;
            if (isSaveChange)
            {
                await SaveChangesAsync();
            }
        }
        /// <summary>
        /// reposiopry.Update(a => a.Name = "ABC", setter => setter.SetProperty(i => i.Age, 18).SetProperty(i => i.Name = "CCC"))
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="setPropertyCalls"></param>
        /// <returns></returns>
        public async Task<int> Update(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            var query = dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);

            }
            return await query.ExecuteUpdateAsync(setPropertyCalls);
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
            var query = dbSet.ApplyConditions(queryHelper, isAsNoTracking: isAsNoTracking);

            return await query.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="queryHelper"></param>
        /// <returns>
        ///  PagedList is a class derived from List<T> and it is used to represent pagination of a list of objects.
        /// </returns>
        public async Task<PagedList<T>> GetWithPagination(QueryHelper<T> queryHelper, bool isAsNoTracking = true)
        {
            var pagedList = new PagedList<T>();

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
