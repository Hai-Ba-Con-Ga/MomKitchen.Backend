using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.Kitchen;
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
                    Includes = new Expression<Func<Kitchen, object>>[]
                    {
                        t => t.Location
                    }
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
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        AreaId = t.AreaId,
                        AreaName = t.Area.Name,
                        Location = new LocationRes()
                        {
                            Id = t.LocationId,
                            Lat = t.Location.Lat,
                            Lng = t.Location.Lng
                        },
                        OwnerId = t.OwnerId,
                        OwnerName = t.Owner.FullName,
                        Status = t.Status
                    },
                    Includes = new Expression<Func<Kitchen, object>>[]
                    {
                        t => t.Area,
                        t => t.Owner,
                        t => t.Location,
                    }
                };

                var kitchen = await _unitOfWork.Kitchen.GetById(kitchenId, queryHelper);

                return Success(kitchen);
            }
            catch (Exception ex)
            {
                return BadRequest<KitchenRes>(ex.Message);
            }
        }

        public async Task<PaginationResponse<KitchenRes>> GetAll(PaginationParameters pagingParam = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Kitchen, KitchenRes>()
                {
                    Selector = t => new KitchenRes
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Address = t.Address,
                        AreaId = t.AreaId,
                        AreaName = t.Area.Name,
                        Location = new LocationRes()
                        {
                            Id = t.LocationId,
                            Lat = t.Location.Lat,
                            Lng = t.Location.Lng
                        },
                        OwnerId = t.OwnerId,
                        OwnerName = t.Owner.FullName,
                        Status = t.Status
                    },
                    Includes = new Expression<Func<Kitchen, object>>[]
                    {
                        t => t.Area,
                        t => t.Owner,
                        t => t.Location,
                    },
                    PaginationParams = pagingParam ??= new PaginationParameters(),
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
