using MK.Domain.Dto;
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
        Task<ResponseObject<DishDetailRes>> GetDishById(Guid dishId);
        Task<PagingResponse<DishRes>> GetAllDish(PagingParameters pagingParam, string[] fields);
        Task<ResponseObject<Guid>> CreateDish(CreateDishReq createReq);
        Task<ResponseObject<bool>> DeleteDish(Guid dishId);
        Task<ResponseObject<bool>> UpdateDish(Guid dishId, UpdateDishReq updateReq);
    }
}
