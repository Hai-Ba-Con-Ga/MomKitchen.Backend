using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationRes>> GetAll();
    }
}
