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
        Task<ResponseObject<UserResponse>> Create(UserRequest userRequest);
        Task<ResponseObject<UserResponse>> Update(Guid id, UserRequest userRequest);
        Task<ResponseObject<bool>> Delete(Guid id);
        Task<ResponseObject<UserResponse>> GetById(Guid id);

        Task<PaginationResponse<UserResponse>> GetAll(PaginationParameters paginationparam = null);
    }
}
