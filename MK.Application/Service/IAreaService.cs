using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IAreaService
    {
        Task<ResponseObject<Guid>> Create(CreateAreaReq req);
        Task<ResponseObject<bool>> Update(Guid areaId, UpdateAreaReq req);
        Task<ResponseObject<bool>> Delete(Guid areaId);
        Task<ResponseObject<GetAreaRes>> GetById(Guid areaId);
        Task<ResponseObject<IEnumerable<GetAreaRes>>> GetAll();
    }
}
