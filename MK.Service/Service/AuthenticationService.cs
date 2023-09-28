using FirebaseAdmin.Auth;
using MK.API.Application.Repository;
using MK.Domain.Common;

namespace MK.Service.Service
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


        public async Task<ResponseObject<User>> GetUserByFirebaseTokenAsync(string token)
        {
            FirebaseToken firebaseToken = await GetFirebaseTokenAsync(token);
            var email = firebaseToken.Claims.GetValueOrDefault("email");
            var name = firebaseToken.Claims.GetValueOrDefault("name");
            if (email is null)
            {
                return new ResponseObject<User>()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Email is null"
                };
            }
            var query = new QueryHelper<User>()
            {
                Filter = x => x.Email == email
            };
            
            var userEntity = (await _userRepository.Get(query)).FirstOrDefault();


            return new ResponseObject<User>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = userEntity
            };
        }
    }
}
