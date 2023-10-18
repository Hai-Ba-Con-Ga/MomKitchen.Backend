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

                var createLocatinResults = await _unitOfWork.Location.CreateAsync(locations, true);

                var area = new Area()
                {
                    Name = req.Name,
                    Boundaries = createLocatinResults.ToArray()
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
                var queryHelper = new QueryHelper<Area>()
                {
                    Selector = i => new Area()
                    {
                        Id = i.Id,
                        Boundaries = i.Boundaries
                    }
                };

                var area = await _unitOfWork.Area.GetById(areaId, queryHelper, false);

                if (area == null)
                {
                    return BadRequest<bool>("Area not found");
                }

                var boundaries = area.Boundaries.ToList();
                boundaries.RemoveAll(i =>
                {
                    if (req.UpdateData.ContainsKey(i))
                    {
                        req.UpdateData.Remove(i);
                        return false;
                    }
                    else
                    {
                        return true; // return true to remove item
                    }
                });

                //create new location and add to boundaries
                var newLocations = req.UpdateData.Select(i => new Location()
                {
                    Id = i.Key,
                    Lat = i.Value.Lat,
                    Lng = i.Value.Lng
                });
                var newLocationIds = await _unitOfWork.Location.CreateAsync(newLocations, isSaveChange: true);
                boundaries.AddRange(newLocationIds);

                //mapping data to update
                area.Boundaries = boundaries.ToArray();
                area.Name = req.Name;

                var updateAreaResult = await _unitOfWork.Area.UpdateAsync(area, isSaveChange: true);

                return Success(updateAreaResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid areaId)
        {
            try
            {
                var delete = await _unitOfWork.Area.SoftDeleteAsync(a => a.Id == areaId);

                return Success(delete > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<ResponseObject<GetAreaRes>> GetById(Guid areaId)
        {
            try
            {
                var area = await _unitOfWork.Area.GetById(areaId, new QueryHelper<Area>());

                if (area == null)
                {
                    return BadRequest<GetAreaRes>("Can not get area with id : " + areaId);
                }

                var locations = await _unitOfWork.Location.Get(new QueryHelper<Location, LocationRes>()
                {
                    Filter = i => area.Boundaries.Contains(i.Id),
                });

                return Success(new GetAreaRes()
                {
                    No = area.No,
                    Id = area.Id,
                    Name = area.Name,
                    Boundaries = locations
                });
            }
            catch (Exception ex)
            {
                return BadRequest<GetAreaRes>(ex.Message);
            }
        }

        public async Task<PagingResponse<GetAreaRes>> GetAll(PagingParameters queryParam)
        {
            try
            {
                var areas = await _unitOfWork.Area.GetWithPagination(new QueryHelper<Area>()
                {
                    PagingParams = queryParam ??= new PagingParameters()
                });

                if (areas == null)
                {
                    return BadRequests<GetAreaRes>("Can not get areas");
                }

                var result = new PagedList<GetAreaRes>();

                foreach (var area in areas)
                {
                    var locations = await _unitOfWork.Location.Get(new QueryHelper<Location, LocationRes>()
                    {
                        Filter = i => area.Boundaries.Contains(i.Id),
                    });

                    if (locations == null)
                    {
                        return BadRequests<GetAreaRes>("Can not get locations for area : " + area.Id);
                    }

                    result.Add(new GetAreaRes()
                    {
                        No = area.No,
                        Id = area.Id,
                        Boundaries = locations,
                        Name = area.Name
                    });
                }

                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequests<GetAreaRes>(ex.Message);
            }
        }
    }
}
