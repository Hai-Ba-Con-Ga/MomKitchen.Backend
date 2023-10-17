using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class MealService : BaseService, IMealService
    {
        public MealService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<PagingResponse<MealRes>> GetMealsByKitchenId(Guid kitchenId, PagingParameters pagingParam)
        {
            try
            {
                var queryHelper = new QueryHelper<Meal, MealRes>()
                {
                    Filter = x => x.KitchenId == kitchenId,
                    PagingParams = pagingParam ??= new PagingParameters(),
                };

                var meals = await _unitOfWork.Meal.GetWithPagination(queryHelper);

                return Success(meals);
            }
            catch (Exception ex)
            {
                return BadRequests<MealRes>(ex.GetExceptionMessage());
            }
        }
    }
}
