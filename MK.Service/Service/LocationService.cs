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

        public async Task<IEnumerable<LocationRes>> GetAll()
        {
            try
            {
                var query = new QueryHelper<Location>()
                {

                };

                //var result = _unitOfWork.Location.Get()

                return await Task.FromResult<IEnumerable<LocationRes>>(new List<LocationRes>());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
