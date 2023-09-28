using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserRequest userRequest);
        Task<UserResponse> Update(Guid id, UserRequest userRequest);
        Task<int> Delete(Guid id);
        Task<UserResponse> GetById(Guid id);



    }
}
