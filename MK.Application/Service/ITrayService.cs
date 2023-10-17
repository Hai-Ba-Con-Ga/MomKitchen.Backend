using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface ITrayService
    {
        Task<PagingResponse<TrayRes>> GetTraysByKitchenId(Guid kitchenId, PagingParameters pagingParam);
    }
}
