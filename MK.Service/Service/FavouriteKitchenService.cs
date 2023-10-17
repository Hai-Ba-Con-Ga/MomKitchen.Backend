using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Response.FavouriteKitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class FavouriteKitchenService : BaseService
    {
        public FavouriteKitchenService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseObject<Guid>> Create(Guid customerId, Guid favouriteKitchen)
        {
            try
            {
                var createReult = await _unitOfWork.FavouriteKitchen.CreateAsync(new FavouriteKitchen
                {
                    CustomerId = customerId,
                    KitchenId = favouriteKitchen
                });

                return Success(createReult);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid customerId, Guid favouriteKitchenId)
        {
            try
            {
                var deleteResult = await _unitOfWork.FavouriteKitchen.SoftDeleteAsync(t => t.CustomerId == customerId 
                                                                                        && t.KitchenId == favouriteKitchenId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<PagingResponse<FavouriteKitchenRes>> Get(PagingParameters pagingParam)
        {
            try
            {
                var result = await _unitOfWork.FavouriteKitchen.GetWithPagination(new QueryHelper<FavouriteKitchen, FavouriteKitchenRes>()
                {
                    PagingParams = pagingParam,
                    Include = t => t.Include(x => x.Kitchen),
                    Selector = t => new FavouriteKitchenRes
                    {
                        CustomerId = t.CustomerId,
                        KitchenId = t.KitchenId,
                        KitchenName = t.Kitchen.Name
                    }
                });

                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequests<FavouriteKitchenRes>(ex.Message);
            }
        }
    }
}
