using MK.Domain.Dto.Response.FavouriteKitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    internal interface IFavouriteKitchenService
    {
        Task<ResponseObject<Guid>> Create(Guid customerId, Guid favouriteKitchen);
        Task<ResponseObject<bool>> Delete(Guid customerId, Guid favouriteKitchenId);
        Task<PaginationResponse<FavouriteKitchenRes>> Get(PaginationParameters pagingParam);
    }
}
