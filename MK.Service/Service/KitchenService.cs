using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.Kitchen;
using MK.Domain.Dto.Response.Customer;
using MK.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class KitchenService : BaseService, IKitchenService
    {
        public KitchenService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseObject<Guid>> Create(CreateKitchenReq req)
        {
            try
            {
                var newKitchen = _mapper.Map<Kitchen>(req);

                var crateReult = await _unitOfWork.Kitchen.CreateAsync(newKitchen, isSaveChange: true);

                return Success(crateReult);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid kitchenId)
        {
            try
            {
                var deleteResult = await _unitOfWork.Kitchen.SoftDeleteAsync(t => t.Id == kitchenId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Update(Guid kitchenId, UpdateKitchenReq req)
        {
            try
            {
                var queryHelper = new QueryHelper<Kitchen>
                {
                    Include = x => x.Include(t => t.Location)
                };

                var kitchen = await _unitOfWork.Kitchen.GetById(kitchenId, queryHelper, isAsNoTracking: false);

                kitchen.Name = req.Name ?? kitchen.Name;
                kitchen.Address = req.Address ?? kitchen.Address;
                kitchen.Status = req.Status ?? kitchen.Status;
                kitchen.Location.Lat = req.Location?.Lat ?? kitchen.Location.Lat;

                var updateResult = await _unitOfWork.Kitchen.UpdateAsync(kitchen, isSaveChange: true);

                return Success(updateResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }

        public async Task<ResponseObject<KitchenRes>> GetById(Guid kitchenId)
        {
            try
            {
                var queryHelper = new QueryHelper<Kitchen, KitchenRes>()
                {
                    Selector = t => new KitchenRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        Location = new LocationRes()
                        {
                            Id = t.LocationId,
                            Lat = t.Location.Lat,
                            Lng = t.Location.Lng
                        },
                        Area = new GetAreaRes()
                        {
                            Id = t.AreaId,
                            Name = t.Area.Name
                        },
                        Owner = new OwnerRes()
                        {
                            OwnerId = t.OwnerId,
                            OwnerName = t.Owner.FullName,
                            OwnerAvatarUrl = t.Owner.AvatarUrl,
                            OwnerEmail = t.Owner.Email,
                        },
                        Status = t.Status,
                        NoOfDish = t.Dishes.Count,
                        NoOfTray = t.Trays.Count,
                        NoOfMeal = t.Meals.Count,
                        Rating = t.Meals.SelectMany(t => t.Orders).Count()
                    },
                    Include = t => t.Include(t => t.Area)
                                    .Include(t => t.Owner)
                                    .Include(t => t.Location)
                                    .Include(t => t.Dishes)
                                    .Include(t => t.Trays)
                                    .Include(t => t.Meals)
                                    .ThenInclude(t => t.Orders),
                };

                var kitchen = await _unitOfWork.Kitchen.GetById(kitchenId, queryHelper);

                return Success(kitchen);
            }
            catch (Exception ex)
            {
                return BadRequest<KitchenRes>(ex.Message);
            }
        }

        public async Task<PagingResponse<KitchenRes>> GetAll(PagingParameters pagingParam = null, string[] fields = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Kitchen, KitchenRes>()
                {
                    Selector = t => new KitchenRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        Area = new GetAreaRes()
                        {
                            Id = t.AreaId,
                            Name = t.Area.Name
                        },
                        Location = new LocationRes()
                        {
                            Id = t.LocationId,
                            Lat = t.Location.Lat,
                            Lng = t.Location.Lng
                        },
                        Owner = new OwnerRes()
                        {
                            OwnerName = t.Owner.FullName,
                            OwnerAvatarUrl = t.Owner.AvatarUrl,
                            OwnerEmail = t.Owner.Email
                        },
                        Status = t.Status,
                        NoOfDish = t.Dishes.Count,
                        NoOfTray = t.Trays.Count,
                        NoOfMeal = t.Meals.Count,
                    },
                    Include = t => t.Include(x => x.Area)
                                    .Include(x => x.Owner)
                                    .Include(x => x.Location)
                                    .Include(t => t.Dishes)
                                    .Include(t => t.Trays)
                                    .Include(t => t.Meals),
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields
                };

                var kitchen = await _unitOfWork.Kitchen.GetWithPagination(queryHelper);


                return Success(kitchen);
            }
            catch (Exception ex)
            {
                return BadRequests<KitchenRes>(ex.Message);
            }
        }

        public async Task<PagingResponse<KitchenRes>> GetKitchensByAreaId(Guid areaId, PagingParameters pagingParam = null, string[] fields = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Kitchen, KitchenRes>()
                {
                    Selector = t => new KitchenRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        Area = new GetAreaRes()
                        {
                            Id = t.AreaId,
                            Name = t.Area.Name
                        },
                        Location = new LocationRes()
                        {
                            Id = t.LocationId,
                            Lat = t.Location.Lat,
                            Lng = t.Location.Lng
                        },
                        Owner = new OwnerRes()
                        {
                            OwnerName = t.Owner.FullName,
                            OwnerAvatarUrl = t.Owner.AvatarUrl,
                            OwnerEmail = t.Owner.Email
                        },
                        Status = t.Status,
                        NoOfDish = t.Dishes.Count,
                        NoOfTray = t.Trays.Count,
                        NoOfMeal = t.Meals.Count,
                    },
                    Include = t => t.Include(x => x.Area)
                                    .Include(x => x.Owner)
                                    .Include(x => x.Location)
                                    .Include(t => t.Dishes)
                                    .Include(t => t.Trays)
                                    .Include(t => t.Meals),
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields,
                    Filter = t => t.AreaId == areaId
                };

                var kitchen = await _unitOfWork.Kitchen.GetWithPagination(queryHelper);


                return Success(kitchen);
            }
            catch (Exception ex)
            {
                return BadRequests<KitchenRes>(ex.Message);
            }
        }

    }
}
