using MK.Domain.Dto.Request.Tray;
using MK.Domain.Dto.Response.Tray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface ITrayService
    {
        Task<PagingResponse<TrayRes>> GetTraysByKitchenId(Guid kitchenId, string searchKey, PagingParameters pagingParam);

        Task<PagingResponse<TrayRes>> GetAll(PagingParameters pagingParam, string[] fields);

        Task<ResponseObject<TrayDetailRes>> GetTrayById(Guid trayId);
        Task<ResponseObject<Guid>> Create(CreateTrayReq req);
        Task<ResponseObject<bool>> Delete(Guid trayId);
        Task<ResponseObject<bool>> Update(Guid trayId, UpdateTrayReq updateData);


    }
}
