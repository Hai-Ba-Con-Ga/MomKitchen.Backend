using Microsoft.EntityFrameworkCore.Metadata;
using MK.Domain.Dto;
using MK.Domain.Dto.Request.Storage;
using MK.Domain.Dto.Response.Dish;
using MK.Domain.Entity;
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
        private readonly IStorageService _storageService;

        public DishService(IUnitOfWork unitOfWork, IMapper mapper,
            IStorageService storageService
        ) : base(unitOfWork, mapper)
        {
            _storageService = storageService;
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

        public async Task<ResponseObject<DishDetailRes>> GetDishById(Guid dishId)
        {
            try
            {
                var dish = await _unitOfWork.Dish.GetById(dishId, new QueryHelper<Dish, DishDetailRes>()
                {
                    Selector = x => new DishDetailRes()
                    {
                        Id = x.Id,
                        No = x.No,
                        Name = x.Name,
                        ImageUrl = x.ImageUrl,
                        Description = x.Description,
                        KitchenId = x.KitchenId,
                        KitchenName = x.Kitchen.Name,
                        Status = x.Status,
                        CreateDate = x.CreatedDate
                    },
                    Include = t => t.Include(x => x.Kitchen)
                });


                return Success(dish);
            }
            catch (Exception ex)
            {
                return BadRequest<DishDetailRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<PagingResponse<DishRes>> GetAllDish(PagingParameters pagingParam, string[] fields)
        {
            try
            {
                var queryHelper = new QueryHelper<Dish, DishRes>()
                {
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields
                };

                var dishes = await _unitOfWork.Dish.GetWithPagination(queryHelper);

                return Success(dishes);
            }
            catch (Exception ex)
            {
                return BadRequests<DishRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<Guid>> CreateDish(CreateDishReq createReq)
        {
            try
            {
                var dish = _mapper.Map<Dish>(createReq);

                var dishId = await _unitOfWork.Dish.CreateAsync(dish, isSaveChange: true);

                return Success(dishId);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> DeleteDish(Guid dishId)
        {
            try
            {
                var deleteResult = await _unitOfWork.Dish.SoftDeleteAsync(d => d.Id == dishId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> UpdateDish(Guid dishId, UpdateDishReq updateReq)
        {
            try
            {
                var dish = await _unitOfWork.Dish.GetById(dishId, isAsNoTracking: false);

                if (dish == null)
                {
                    return BadRequest<bool>("Dish not found");
                }

                dish = _mapper.Map(updateReq, dish);

                var updateResult = await _unitOfWork.Dish.UpdateAsync(dish, isSaveChange: true);

                return Success(updateResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }
    }
}
