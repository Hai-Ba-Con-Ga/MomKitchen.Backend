using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Builder;
using MK.API.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using MK.Domain.Entity;
using System.ComponentModel;

namespace MK.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(IGenericRepository<User> userRepository,
                       IGenericRepository<Role> roleRepository,
                                  ITokenService tokenService,
                                             IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public string GenerateToken(UserResponse user)
        {
            return _tokenService.GetToken(user);
        }

        public async Task<FirebaseToken> GetFirebaseTokenAsync(string token)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }


        public async Task<LoginResponse?> GetUserByFirebaseTokenAsync(LoginRequest loginRequest)
        {
            FirebaseToken firebaseToken = await GetFirebaseTokenAsync(loginRequest.IdToken);
            //get info from firebase token
            var email = firebaseToken.Claims.GetValueOrDefault("email")?.ToString() ;
            var phone = firebaseToken.Claims.GetValueOrDefault("phone_number")?.ToString();
            var birthday = firebaseToken.Claims.GetValueOrDefault("birthday")?.ToString();
            var avatar = firebaseToken.Claims.GetValueOrDefault("picture")?.ToString();
            var name = firebaseToken.Claims.GetValueOrDefault("name")?.ToString();


            LoginResponse loginResponse = new();
            if (email is null && phone is null)
            {
                return null;
            }

            var query = new QueryHelper<User>()
            {
                Filter = x => x.Email == email || x.Phone == phone
            };

            var user =  (await _userRepository.Get(query)).FirstOrDefault();
            
            //check user exist
            if (user is null)
            {
                loginResponse.IsFirstTime = true;
                var role = await _roleRepository.FirstOrDefaultAsync(x => x.Name == "Customer");
                if (role is null)
                {
                    role = new Role
                    {
                        Name = "Customer"
                    };
                    await _roleRepository.CreateAsync(role);

                }
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
                await _userRepository.CreateAsync(newUser, true);
                UserResponse userResponse = new UserResponse()
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    Phone = newUser.Phone,
                    AvatarUrl = newUser.AvatarUrl,
                    FullName = newUser.FullName,
                    RoleName = newUser.Role.Name,
                };
                loginResponse.User = userResponse;
                //generate token
                loginResponse.Token = GenerateToken(userResponse);

            }
            else
            {
                loginResponse.IsFirstTime = false;
                var role = await _roleRepository.GetById(user.RoleId, new QueryHelper<Role>());
                UserResponse userResponse = new UserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Phone = user.Phone,
                    AvatarUrl = user.AvatarUrl,
                    FullName = user.FullName,
                    RoleName = role.Name,
                };

                
                //update fcm token
                user.FcmToken.Add(loginRequest.FcmToken);
                await _userRepository.SaveChangesAsync();

                loginResponse.Token = GenerateToken(userResponse);
            }
            
            return loginResponse;
        }

        public async Task<Boolean> Logout(Guid userId, string fcmToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                return false;
            }
            user.FcmToken.Remove(fcmToken);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
