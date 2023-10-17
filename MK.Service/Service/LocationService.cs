using Microsoft.EntityFrameworkCore.Query;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Response;
using MK.Infrastructure.Common;
using MK.Infrastructure.Repository;
using MK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class LocationService : BaseService, ILocationService
    {
        public LocationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseObject<Guid>> Create(CreateLocationReq req)
        {
            try
            {
                var result = await _unitOfWork.Location.CreateAsync(_mapper.Map<Location>(req), isSaveChange: true);

                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid locationId)
        {
            try
            {
                var result = await _unitOfWork.Location.SoftDeleteAsync(t => t.Id.Equals(locationId));
                return Success(result > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<PagingResponse<LocationRes>> GetAll(PagingParameters pagingParam = null)
        {
            try
            {
                var query = new QueryHelper<Location, LocationRes>()
                {
                    PagingParams = pagingParam ??= new PagingParameters(),
                };

                var resultQuery = await _unitOfWork.Location.GetWithPagination(query);

                return Success(resultQuery);
            }
            catch (Exception ex)
            {
                return BadRequests<LocationRes>(ex.Message);
            }
        }

        public async Task<ResponseObject<LocationRes>> GetById(Guid locationId)
        {
            try
            {
                var query = new QueryHelper<Location, LocationRes>()
                {
                    Selector = t => new LocationRes()
                    {
                        Lat = t.Lat
                    },
                };

                var result = await _unitOfWork.Location.GetById<LocationRes>(locationId, query);

                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequest<LocationRes>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Update(Guid locationId, UpdateLocationReq req)
        {
            try
            {
                var result = await _unitOfWork.Location.UpdateAsync(t => t.Id.Equals(locationId), 
                    t => t.SetProperty(x => x.Lat, req.Lat).SetProperty(x => x.Lng, req.Lng));
                return Success(result > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }
    }
}
