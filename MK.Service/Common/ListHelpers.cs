using MK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Common
{
    public static class ListHelpers
    {
        public static PagedList<TDestination>? PagedListAdapt<TSource, TDestination>(this object source) where TDestination : class where TSource : class
        {
            try
            {
                var pagedListSource = source as PagedList<TSource>;

                if (pagedListSource == null)
                {
                    throw new ArgumentException("Invalid source type");
                }

                var pagedListDestination = pagedListSource.Adapt<PagedList<TDestination>>();

                pagedListDestination.TotalCount = pagedListSource.TotalCount;
                pagedListDestination.TotalPages = pagedListSource.TotalPages;
                pagedListDestination.PageSize = pagedListSource.PageSize;
                pagedListDestination.CurrentPage = pagedListSource.CurrentPage;

                return pagedListDestination;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid source type");
            }
        }
    }
}
