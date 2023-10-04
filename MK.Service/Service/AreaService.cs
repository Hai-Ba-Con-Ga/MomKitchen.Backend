using MK.Domain.Dto.Response.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class AreaService : BaseService, IAreaService
    {
        public AreaService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseObject<Guid>> Create(CreateAreaReq req)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var locations = _mapper.Map<IEnumerable<Location>>(req.Boundaries);

                await _unitOfWork.Location.CreateAsync(locations, true);

                var area = new Area()
                {
                    Name = req.Name,
                    Boundaries = locations.Select(l => l.Id).ToArray()
                };

                var result = await _unitOfWork.Area.CreateAsync(area, true);

                await _unitOfWork.CommitTransactionAsync();

                return Success(result);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RolebackTransactionAsync();
                return BadRequest<Guid>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Update(Guid areaId, UpdateAreaReq req)
        {
            try
            {
                var queryHelper = new QueryHelper<Area, AreaQueryResponse>();

                var area = await _unitOfWork.Area.GetById(areaId, queryHelper);

                if (area == null)
                {
                    return BadRequest<bool>("Area not found");
                }

                var queryLocation = new QueryHelper<Location>()
                {
                    Filter = l => area.Boundaries.Contains(l.Id)
                };

                var locations = (await _unitOfWork.Location.Get(queryLocation, false)).ToList();

                var updateLocationIds = req.UpdateData.Keys.ToList();

                locations.RemoveAll(l => updateLocationIds.Contains(l.Id) == false);

                foreach (var updateLocationId in updateLocationIds)
                {
                    var index = locations.FindIndex(0, l => l.Id == updateLocationId);

                    if (index != -1)
                    {
                        locations.Add(new Location()
                        {
                            Id = updateLocationId,
                            Lat = req.UpdateData[updateLocationId].Lat,
                            Lng = req.UpdateData[updateLocationId].Lng
                        });
                    }
                    else
                    {
                        locations[index].Lat = req.UpdateData[updateLocationId].Lat;
                        locations[index].Lng = req.UpdateData[updateLocationId].Lng;
                    }
                }

                var resultUpdate = await _unitOfWork.SaveChangeAsync();

                return Success(resultUpdate > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }
    }
}
