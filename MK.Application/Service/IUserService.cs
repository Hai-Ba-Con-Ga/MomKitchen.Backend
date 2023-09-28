using MK.Domain.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IUserService
    {
        Task<User> SignUpUserAsync(FirstTimeRequest firstTimeRequest);
    }
}
