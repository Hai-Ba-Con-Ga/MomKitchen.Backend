using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using MK.API.Application.Repository;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using MK.Domain.Entity;
using System.ComponentModel;

namespace MK.Service.Service
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        
        private readonly ITokenService _tokenService;


        public AuthenticationService (IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : base(unitOfWork, mapper)
        {

            _tokenService = tokenService;

        }
        

        public string GenerateToken(UserResponse user)
        {
            return _tokenService.GetToken(user);
        }

        public async Task<FirebaseToken> GetFirebaseTokenAsync(string token)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }


        public async Task<ResponseObject<LoginResponse>> GetUserByFirebaseTokenAsync(LoginRequest loginRequest)
        {
            FirebaseToken firebaseToken = await GetFirebaseTokenAsync(loginRequest.IdToken);
            //get info from firebase token
            var email = firebaseToken.Claims.GetValueOrDefault("email")?.ToString() ;
            var phone = firebaseToken.Claims.GetValueOrDefault("phone_number")?.ToString();
            var birthday = firebaseToken.Claims.GetValueOrDefault("birthday")?.ToString();
            var avatar = firebaseToken.Claims.GetValueOrDefault("picture")?.ToString();
            var name = firebaseToken.Claims.GetValueOrDefault("name")?.ToString();

            var role = await _unitOfWork.Role.FirstOrDefaultAsync(x => x.Name.Equals(loginRequest.RoleName));


            LoginResponse loginResponse = new();
            if (email is null && phone is null)
            {
                return BadRequest<LoginResponse>("Invalid token");
            }

            var query = new QueryHelper<User>()
            {
                Filter = x =>( x.Email == email || x.Phone == phone),
                Includes = new Expression<Func<User, object>>[] { x => x.Role },
            };

            var user =  (await _unitOfWork.User.Get(query)).FirstOrDefault();
            
            //check user exist
            if (user is null)
            {
                loginResponse.IsFirstTime = true;
                //create new user
                User newUser = new()
                {
                    Email = email ?? "",
                    Phone = phone ?? "",
                    AvatarUrl = avatar ?? "",
                    FullName = name ?? "",
                    Role = role,
                    FcmToken = new List<string>() { loginRequest.FcmToken },
                };
                await _unitOfWork.User.CreateAsync(newUser, true);
                UserResponse userResponse = _mapper.Map<UserResponse>(newUser);
                userResponse.Role = newUser.Role;
                
                loginResponse.User = userResponse;
                //generate token
                loginResponse.Token = GenerateToken(userResponse);

            }
            else
            {
                loginResponse.IsFirstTime = false;
                UserResponse userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Role = user.Role;
                
                //update fcm token
                user.FcmToken.Add(loginRequest.FcmToken);
                await _unitOfWork.User.SaveChangesAsync();

                loginResponse.Token = GenerateToken(userResponse);
            }
            
            return Success(loginResponse);
        }

        public async Task<ResponseObject<bool>> Logout(Guid userId, string fcmToken)
        {
            var user = await _unitOfWork.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                return Success(false);
            }
            user.FcmToken.Remove(fcmToken);
            await _unitOfWork.User.SaveChangesAsync();
            return Success(true);
        }
    }
}
