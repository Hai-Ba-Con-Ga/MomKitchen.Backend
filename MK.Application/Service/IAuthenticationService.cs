using FirebaseAdmin.Auth;
using MK.Domain.Dto.Request.User;
using MK.Domain.Dto.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IAuthenticationService 
    {
        string GenerateToken(UserRes user);
        Task<ResponseObject<LoginRes>> GetUserByFirebaseTokenAsync(LoginReq loginRequest);

        Task<FirebaseToken> GetFirebaseTokenAsync(string token);
        Task<ResponseObject<bool>> Logout(Guid userId, string fcmToken);

        Task<ResponseObject<UserRes>> Get(Guid userId);
        Task<ResponseObject<UserRes>> Update(Guid userId, UpdateUserReq userRequest);
    }
}
