using MK.Domain.Dto.Request.Meal;
using MK.Domain.Dto.Response.Meal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IMealService
    {
        Task<PagingResponse<MealRes>> GetMealsByKitchenId(Guid kitchenId, string searchKey, PagingParameters pagingParam);
        Task<ResponseObject<MealDetailRes>> GetMealById(Guid mealId);
        Task<PagingResponse<MealRes>> GetAll(PagingParameters pagingParam, string[] fields);
        Task<ResponseObject<Guid>> CreateMeal(CreateMealReq createData);
        Task<ResponseObject<bool>> DeleteMeal(Guid mealId);
        Task<ResponseObject<bool>> UpdateMeal(Guid mealId, UpdateMealReq updateData);
    }
}
