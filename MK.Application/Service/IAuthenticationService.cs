using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IAuthenticationService 
    {
        string GenerateToken(User user);
        Task<ResponseObject<User>> GetUserByFirebaseTokenAsync(string token);

        Task<FirebaseToken> GetFirebaseTokenAsync(string token);
    }
}
