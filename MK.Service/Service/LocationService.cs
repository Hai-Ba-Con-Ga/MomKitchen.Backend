using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Response;
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

        public async Task<ResponseObject<IEnumerable<LocationRes>>> GetAll()
        {
            try
            {
                var query = new QueryHelper<Location>()
                {
                    SelectedFields = new string[] { "Lat" }
                };

                var result = await _unitOfWork.Location.Get(query);

                return Success(_mapper.Map<IEnumerable<LocationRes>>(result));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
