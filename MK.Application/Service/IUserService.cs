using MK.Domain.Dto.Request.User;
using MK.Domain.Dto.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IUserService
    {
        Task<ResponseObject<UserResponse>> Create(CreateUserRequest userRequest);
        Task<ResponseObject<UserResponse>> Update(Guid id, UpdateUserRequest userRequest);
        Task<ResponseObject<bool>> Delete(Guid id);
        Task<ResponseObject<UserResponse>> GetById(Guid id);

        Task<PaginationResponse<UserResponse>> GetAll(PaginationParameters paginationparam = null);
        Task<ResponseObject<bool>> UpdateRole(Guid userId, string roleName);
    }
}
