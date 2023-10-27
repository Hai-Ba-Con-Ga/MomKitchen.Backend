

using MK.Domain.Dto.Request.Tray;
using MK.Domain.Dto.Response.Dish;
using MK.Domain.Dto.Response.Tray;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace MK.Service.Service
{
    public class TrayService : BaseService, ITrayService
    {

        public TrayService(IUnitOfWork unitOfWork, IMapper mapper
        ) : base(unitOfWork, mapper)
        {
        }

        public async Task<PagingResponse<TrayRes>> GetAll(PagingParameters pagingParam, string[] fields)
        {
            try
            {
                var queryHelper = new QueryHelper<Tray, TrayRes>()
                {
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields
                };

                var trays = await _unitOfWork.Tray.GetWithPagination(queryHelper);

                return Success(trays);
            }
            catch (Exception ex)
            {
                return BadRequests<TrayRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<TrayDetailRes>> GetTrayById(Guid trayId)
        {
            try
            {
                var queryHelper = new QueryHelper<Tray, TrayDetailRes>()
                {
                    Selector = t => new TrayDetailRes()
                    {
                        Id = t.Id,
                        No = t.No,
                        Name = t.Name,
                        Description = t.Description,
                        ImgUrl = t.ImgUrl,
                        Price = t.Price,
                        KitchenId = t.KitchenId,
                        KitchenName = t.Kitchen.Name,
                        Dishies = t.Dishies.Select(d => _mapper.Map<DishRes>(d)),
                        CreatedDate = t.CreatedDate,
                    },
                    Include = t => t.Include(t => t.Kitchen)
                                    .Include(t => t.Dishies),
                };

                var trays = await _unitOfWork.Tray.GetById(trayId, queryHelper);

                return Success(trays);
            }
            catch (Exception ex)
            {
                return BadRequest<TrayDetailRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<PagingResponse<TrayRes>> GetTraysByKitchenId(Guid kitchenId, string searchKey, PagingParameters pagingParam)
        {
            try
            {
                var queryHelper = new QueryHelper<Tray, TrayRes>()
                {
                    Filter = x => x.KitchenId == kitchenId && (searchKey == null || x.Name.Contains(searchKey)),
                    PagingParams = pagingParam ??= new PagingParameters(),
                    Include = t => t.Include(t=>t.Dishies),
                    Selector = t => new TrayRes(){
                        Description = t.Description,
                        Price = t.Price,
                        Id = t.Id,
                        ImgUrl = t.ImgUrl,
                        Name = t.Name,
                        Dishes = t.Dishies.Select(d => _mapper.Map<DishRes>(d)),

                        }
                };

                var trays = await _unitOfWork.Tray.GetWithPagination(queryHelper);

                return Success(trays);
            }
            catch (Exception ex)
            {
                return BadRequests<TrayRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<Guid>> Create(CreateTrayReq req)
        {
            try
            {
                var tray = _mapper.Map<Tray>(req);

                var dishies = (await _unitOfWork.Dish.Get(new QueryHelper<Dish>()
                {
                    Filter = d => req.DishIds.Contains(d.Id),
                }, isAsNoTracking: false)).ToList();

                if (dishies == null || dishies.Any() == false)
                {
                    return BadRequest<Guid>("Dish not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                var trayId = await _unitOfWork.Tray.CreateAsync(tray, isSaveChange: true);

                tray.Dishies = dishies;

                var updateResult = await _unitOfWork.Tray.UpdateAsync(tray, isSaveChange: true);

                await _unitOfWork.CommitTransactionAsync();
                return Success(trayId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RolebackTransactionAsync();

                return BadRequest<Guid>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid trayId)
        {
            try
            {
                var deleteResult = await _unitOfWork.Tray.SoftDeleteAsync(d => d.Id == trayId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> Update(Guid trayId, UpdateTrayReq updateData)
        {
            try
            {
                var tray = await _unitOfWork.Tray.GetById(trayId, new QueryHelper<Tray>()
                { 
                    Include = t => t.Include(t => t.Dishies),
                }, isAsNoTracking: false);


                if (tray == null)
                {
                    return BadRequest<bool>("Tray not found");
                }

                tray = _mapper.Map(updateData, tray);

                var dishies = (await _unitOfWork.Dish.Get(new QueryHelper<Dish>()
                {
                    Filter = d => updateData.DishIds.Contains(d.Id),
                }, isAsNoTracking: false)).ToList();

                tray.Dishies = dishies;

                var updateResult = await _unitOfWork.Tray.UpdateAsync(tray, isSaveChange: true);

                return Success(updateResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }
    }
}
