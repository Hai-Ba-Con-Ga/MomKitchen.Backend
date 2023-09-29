using MK.Domain.Dto.Request.Location;
using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface ILocationService
    {
        Task<PaginationResponse<LocationRes>> GetAll(PaginationParameters pagingParam = null);
        Task<ResponseObject<Guid>> Create(CreateLocationReq req);
        Task<ResponseObject<LocationRes>> GetById(Guid locationId);
        Task<ResponseObject<bool>> Delete(Guid locationId);
        Task<ResponseObject<bool>> Update(Guid locationId, UpdateLocationReq req);
    }
}
