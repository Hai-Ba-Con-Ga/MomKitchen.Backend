using MK.Domain.Dto.Response.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IDishService
    {
        Task<PagingResponse<DishRes>> GetDishesByKitchenId(Guid kitchenId, PagingParameters pagingParam);
    }
}
