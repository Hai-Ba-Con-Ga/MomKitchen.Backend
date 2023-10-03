using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var result = await _unitOfWork.Area.CreateAsync(_mapper.Map<Area>(req), true);
                return Success(result);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.Message);
            }
        }

        //public async Task<ResponseObject<bool>> Update(UpdateAreaReq res)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest<bool>(ex.Message);
        //    }
        //}
    }
}
