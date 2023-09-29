using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.Location;
using MK.Domain.Dto.Response;
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

        public async Task<PaginationResponse<LocationRes>> GetAll(PaginationParameters pagingParam = null)
        {
            try
            {
                var query = new QueryHelper<Location>()
                {
                    Selector = t => new Location()
                    {
                        Lat = t.Lat,
                        Lng = t.Lng,
                    },
                    PaginationParams = pagingParam ??= new PaginationParameters(),
                };

                var resultQuery = await _unitOfWork.Location.GetWithPagination(query);

                var result = resultQuery.PagedListAdapt<Location, LocationRes>();

                return Success(result);
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
                var query = new QueryHelper<Location, LocationRes>();

                var result = await _unitOfWork.Location.GetById<LocationRes>(locationId, query);

                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequest<LocationRes>(ex.Message);
            }
        }
    }
}
