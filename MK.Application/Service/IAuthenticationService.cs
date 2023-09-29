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
        Task<LoginResponse> GetUserByFirebaseTokenAsync(LoginRequest loginRequest);

        Task<FirebaseToken> GetFirebaseTokenAsync(string token);
        Task<Boolean> Logout(Guid userId, string fcmToken);
    }
}
