using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IMealService
    {
        Task<PagingResponse<MealRes>> GetMealsByKitchenId(Guid kitchenId, PagingParameters pagingParam);
    }
}
