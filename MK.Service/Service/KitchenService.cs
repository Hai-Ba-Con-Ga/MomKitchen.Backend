using MK.Application.Cache;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Constant;
using MK.Domain.Dto.Request.Kitchen;
using MK.Domain.Dto.Request.Order;
using MK.Domain.Dto.Response.Customer;
using MK.Infrastructure.Cache;
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
        public KitchenService(IUnitOfWork unitOfWork, IMapper mapper, ICacheManager cacheManager) : base(unitOfWork, mapper, cacheManager)
        {
        }

        public async Task<ResponseObject<Guid>> Create(CreateKitchenReq req)
        {
            try
            {
                var newKitchen = _mapper.Map<Kitchen>(req);

                var crateReult = await _unitOfWork.Kitchen.CreateAsync(newKitchen, isSaveChange: true);

                await _cacheManager.RemoveAsync(AppConstant.CacheKey_KitchenPage1);

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

                await _cacheManager.RemoveAsync(AppConstant.CacheKey_KitchenPage1);
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
                kitchen.Location.Lng = req.Location?.Lng ?? kitchen.Location.Lng;

                var updateResult = await _unitOfWork.Kitchen.UpdateAsync(kitchen, isSaveChange: true);

                await _cacheManager.RemoveAsync(AppConstant.CacheKey_KitchenPage1);

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
                        ImgUrl = t.ImgUrl,
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

        public async Task<PagingResponse<KitchenRes>> GetAll(GetKitchenReq getReq, PagingParameters pagingParam = null)
        {
            try
            {
                if (pagingParam.PageNumber == 1)
                {
                    var (result, kitchenRes) = await _cacheManager.GetAsync<PagingResponse<KitchenRes>>(AppConstant.CacheKey_KitchenPage1);
                    if (kitchenRes != null)
                    {
                        return kitchenRes;
                    }
                }

                var queryHelper = new QueryHelper<Kitchen, KitchenRes>()
                {
                    Selector = t => new KitchenRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        ImgUrl = t.ImgUrl,
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
                            OwnerId = t.OwnerId,
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
                    OrderByFields = getReq?.OrderBy,
                    Filter = t => (getReq.KeySearch == null
                                        || t.No.ToString() == getReq.KeySearch
                                        || t.Name.Contains(getReq.KeySearch)
                                        || t.Address.Contains(getReq.KeySearch))
                                    && (t.CreatedDate.Date >= getReq.FromDate && t.CreatedDate <= getReq.ToDate)
                };

                var kitchen = await _unitOfWork.Kitchen.GetWithPagination(queryHelper);

                if (pagingParam.PageNumber == 1)
                {
                    await _cacheManager.SetAsync(AppConstant.CacheKey_KitchenPage1, Success(kitchen));
                }

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
                        ImgUrl = t.ImgUrl,
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
                            OwnerId = t.OwnerId,
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

        public async Task<PagingResponse<KitchenRes>> GetKitchensByUserId(Guid userId, PagingParameters pagingParam = null, string[] fields = null)
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
                        ImgUrl = t.ImgUrl,
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
                            OwnerId = t.OwnerId,
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
                    Filter = t => t.OwnerId == userId
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
