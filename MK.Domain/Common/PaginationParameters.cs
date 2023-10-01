namespace MK.Domain.Common
{
    public class PaginationParameters
    {
        const int maxPageSize = 50;
        private int _pageSize = 0;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public PaginationParameters()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationParameters(int? pageNumer, int? pageSize)
        {
            PageNumber = pageNumer ?? 1;
            PageSize = pageSize ?? 10;
        }

    }
}
