using MK.Domain.Dto.Request.Meal;
using MK.Domain.Dto.Response.Meal;
using MK.Domain.Dto.Response.Tray;
using MK.Domain.Entity;
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

        public async Task<ResponseObject<MealDetailRes>> GetMealById(Guid mealId)
        {
            try
            {

                var meal = await _unitOfWork.Meal.GetById(mealId, new QueryHelper<Meal, MealDetailRes>()
                {
                    Include = x => x.Include(x => x.Kitchen).Include(x => x.Tray),
                    Selector = x => _mapper.Map<MealDetailRes>(x)
                });


                if (meal == null)
                {
                    return BadRequest<MealDetailRes>("Meal not found");
                }

                return Success(meal);
            }
            catch (Exception ex)
            {
                return BadRequest<MealDetailRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<PagingResponse<MealRes>> GetAll(PagingParameters pagingParam, string[] fields)
        {
            try
            {
                var queryHelper = new QueryHelper<Meal, MealRes>()
                {
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields
                };

                var meals = await _unitOfWork.Meal.GetWithPagination(queryHelper);

                return Success(meals);
            }
            catch (Exception ex)
            {
                return BadRequests<MealRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<Guid>> CreateMeal(CreateMealReq createData)
        {
            try
            {
                var meal = _mapper.Map<Meal>(createData);

                var createResult = await _unitOfWork.Meal.CreateAsync(meal, isSaveChange: true);

                return Success(createResult);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> DeleteMeal(Guid mealId)
        {
            try
            {
                if (!await _unitOfWork.Meal.IsExist(t => t.Id == mealId))
                {
                    return BadRequest<bool>($"Meal {mealId} is not exist");
                }

                var deleteResult = await _unitOfWork.Meal.SoftDeleteAsync(t => t.Id == mealId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> UpdateMeal(Guid mealId, UpdateMealReq updateData)
        {
            try
            {
                var meal = await _unitOfWork.Meal.GetById(mealId, isAsNoTracking: false);

                if (meal == null)
                {
                    return BadRequest<bool>("Meal not found");
                }

                meal = _mapper.Map(updateData, meal);

                var updateResult = await _unitOfWork.Meal.UpdateAsync(meal, isSaveChange: true);

                return Success(updateResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }
    }
}
