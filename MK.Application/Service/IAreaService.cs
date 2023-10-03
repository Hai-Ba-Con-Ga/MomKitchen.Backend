using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IAreaService
    {
        public Task<ResponseObject<Guid>> Create(CreateAreaReq req);
    }
}
