using FirebaseAdmin.Auth;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IAuthenticationService 
    {
        string GenerateToken(UserResponse user);
        Task<ResponseObject<LoginResponse>> GetUserByFirebaseTokenAsync(LoginRequest loginRequest);

        Task<FirebaseToken> GetFirebaseTokenAsync(string token);
        Task<ResponseObject<bool>> Logout(Guid userId, string fcmToken);
    }
}
