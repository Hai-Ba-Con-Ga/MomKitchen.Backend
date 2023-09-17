

namespace MK.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;
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
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        /// <summary>
        /// Add a list of entities to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task CreateAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete immediately by condition predicate without SaveChanges action 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Func<T, bool> predicate)
        {
            return await dbSet.Where(predicate).AsQueryable().ExecuteDeleteAsync();
        }
        /// <summary>
        /// Update IsDeleted to true by condition predicate without SaveChanges action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(Func<T, bool> predicate)
        {
            //T _entity = await GetByIdAsync(id);
            //if (_entity != null)
            //{
            //    _entity.is_deleted = true;
            //    await UpdateAsync(_entity);
            //}

            return await dbSet.Where(predicate)
                                    .AsQueryable()
                                    .ExecuteUpdateAsync(setter => setter.SetProperty(e => e.IsDeleted, false));
        }
        #endregion Delete

        #region Update
        /// <summary>
        ///  Change stated of entity to Modified (mark this entity will update), need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            dbContext.Attach(entity).State = EntityState.Modified;
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
        public async Task<T> GetById(Guid id, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes)
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
                return await query.Includes(includes).SingleOrDefaultAsync();
            }
        }
        /// <summary>
        /// Get all entities are active and match condition predicate, this function is AsNoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetWithCondition(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes)
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

                return query.Includes(includes);
            }
        }

        public async Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams)
        {
            PagedList<T> pagedRequests = new PagedList<T>();
            if (pagingParams.PageSize <= 0 || pagingParams.PageNumber <= 0)
            {
                throw new ArgumentException("Page number or page size must be greater than 0");
            }
            else
            {
                await pagedRequests.LoadData(dataQuery.Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreatedDate), pagingParams.PageNumber, pagingParams.PageSize);
            }

            return pagedRequests;

        }

        public async Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams, Expression<Func<T, bool>> predicate)
        {
            PagedList<T> pagedRequests = new PagedList<T>();

            if (pagingParams.PageSize <= 0 || pagingParams.PageNumber <= 0)
            {
                throw new ArgumentException("Page number or page size must be greater than 0");
            }
            else
            {
                await pagedRequests.LoadData(dataQuery.Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreatedDate), pagingParams.PageNumber, pagingParams.PageSize, predicate);
            }
            return pagedRequests;
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

        //public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        //{
        //    List<T> list;
        //    var query = dbSet.AsQueryable();
        //    foreach (var property in navigationProperties)
        //    {
        //        query = query.Include(property);
        //    }
        //    list = await query.Where(predicate).AsNoTracking().ToListAsync();
        //    return list;
    }
}
