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
        Task<ResponseObject<UserRes>> Create(CreateUserReq userRequest);
        Task<ResponseObject<UserRes>> Update(Guid id, UpdateUserReq userRequest);
        Task<ResponseObject<bool>> Delete(Guid id);
        Task<ResponseObject<UserRes>> GetById(Guid id);
        Task<PagingResponse<UserRes>> GetAll(string roleName, string searchKey, PagingParameters paginationparam = null);
        Task<ResponseObject<bool>> UpdateRole(Guid userId, string roleName);
    }
}
