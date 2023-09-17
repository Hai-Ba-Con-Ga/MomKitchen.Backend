
namespace MK.Domain.Common
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 0;
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList()
        {
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public async Task LoadData(IQueryable<T> queryList, int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                throw new ArgumentException("Page number or page size must be greater than 0");
            }

            var items = await queryList
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            this.TotalCount = items.Count();
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);

            this.AddRange(items);
        }
        /// <summary>
        /// Load with condition predicate
        /// </summary>
        /// <param name="queryList"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task LoadData(IQueryable<T> queryList, int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
        {
            var items = new List<T>();

            if (pageNumber < 0 || pageSize < 0)
            {
                throw new ArgumentException("Page number or page size must be greater than 0");
            }

            items = await queryList.Where(predicate)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalCount = items.Count();
            this.TotalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);
            this.AddRange(items);
        }

    }
}
