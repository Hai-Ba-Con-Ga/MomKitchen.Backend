using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MK.Application.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IGenericRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public string GenerateToken(User user)
        {
            return _tokenService.GetToken(user);
        }

        public async Task<FirebaseToken> GetFirebaseTokenAsync(string token)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }


        public async Task<User> GetUserByFirebaseTokenAsync(string token)
        {
            FirebaseToken firebaseToken = await GetFirebaseTokenAsync(token);
            var email = firebaseToken.Claims.GetValueOrDefault("email");
            var name = firebaseToken.Claims.GetValueOrDefault("name");
            if (email is null)
            {
                throw new Exception("Email is null");
            }

            User userEntity = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
            return userEntity;
        }
    }
}
