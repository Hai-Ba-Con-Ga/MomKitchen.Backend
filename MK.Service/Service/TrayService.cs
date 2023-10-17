

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class TrayService : BaseService, ITrayService
    {
        public TrayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }


        public async Task<PagingResponse<TrayRes>> GetTraysByKitchenId(Guid kitchenId, PagingParameters pagingParam)
        {
            try
            {
                var queryHelper = new QueryHelper<Tray, TrayRes>()
                {
                    Filter = x => x.KitchenId == kitchenId,
                    PagingParams = pagingParam ??= new PagingParameters(),
                };

                var trays = await _unitOfWork.Tray.GetWithPagination(queryHelper);

                return Success(trays);
            }
            catch (Exception ex)
            {
                return BadRequests<TrayRes>(ex.GetExceptionMessage());
            }
        }
    }
}
