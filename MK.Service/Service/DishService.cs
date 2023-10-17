using MK.Domain.Dto.Response.Dish;
using MK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class DishService : BaseService, IDishService
    {
        public DishService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<PagingResponse<DishRes>> GetDishesByKitchenId(Guid kitchenId, PagingParameters pagingParam)
        {
            try
            {
                var queryHelper = new QueryHelper<Dish, DishRes>()
                {
                    Filter = x => x.KitchenId == kitchenId,
                    PagingParams = pagingParam ??= new PagingParameters(),
                };

                var dishes = await _unitOfWork.Dish.GetWithPagination(queryHelper);

                return Success(dishes);
            }
            catch (Exception ex)
            {
                return BadRequests<DishRes>(ex.GetExceptionMessage());
            }
        }
    }
}
