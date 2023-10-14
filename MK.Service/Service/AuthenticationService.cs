using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using MK.API.Application.Repository;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.User;
using MK.Domain.Dto.Response.User;
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
            try
            {
                FirebaseToken firebaseToken = await GetFirebaseTokenAsync(loginRequest.IdToken);
                //get info from firebase token
                var email = firebaseToken.Claims.GetValueOrDefault("email")?.ToString();
                var phone = firebaseToken.Claims.GetValueOrDefault("phone_number")?.ToString();
                var birthday = firebaseToken.Claims.GetValueOrDefault("birthday")?.ToString();
                var avatar = firebaseToken.Claims.GetValueOrDefault("picture")?.ToString();
                var name = firebaseToken.Claims.GetValueOrDefault("name")?.ToString();

                var role = await _unitOfWork.Role.GetFirstOrDefaultAsync(x => x.Name.Equals(loginRequest.RoleName));
                if (role is null)
                {
                    return BadRequest<LoginResponse>("Invalid role");
                }

                LoginResponse loginResponse = new();
                if (email is null && phone is null)
                {
                    return BadRequest<LoginResponse>("Invalid token");
                }

                var query = new QueryHelper<User>()
                {
                    Filter = x => (x.Email == email || x.Phone == phone),
                    Includes = new Expression<Func<User, object>>[] { x => x.Role },
                };

                var user = (await _unitOfWork.User.Get(query, false)).FirstOrDefault();

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
                        RoleId = role.Id,
                        FcmToken = new List<string>() { loginRequest.FcmToken },
                    };
                    await _unitOfWork.User.CreateAsync(newUser, true);

                    //create customer
                    if (role.Name.Equals("Customer"))
                    {
                        Customer customer = new()
                        {
                            UserId = newUser.Id
                        };
                        await _unitOfWork.Customer.CreateAsync(customer, true);
                    }
                    
                    UserResponse userResponse = _mapper.Map<UserResponse>(newUser);
                    userResponse.Role = role;


                    loginResponse.User = userResponse;
                    //generate token
                    loginResponse.Token = GenerateToken(userResponse);
                    

                }
                else
                {
                    loginResponse.IsFirstTime = false;
                    //update fcm token
                    if (!user.FcmToken.Contains(loginRequest.FcmToken))
                    {
                        user.FcmToken.Add(loginRequest.FcmToken);

                        await _unitOfWork.User.SaveChangesAsync();
                        
                    }
                    UserResponse userResponse = _mapper.Map<UserResponse>(user);
                    userResponse.Role = user.Role;


                    loginResponse.Token = GenerateToken(userResponse);
                    loginResponse.User = userResponse;
                }

                return Success(loginResponse);

            }
            catch (Exception e)
            {
                return BadRequest<LoginResponse>(e.Message);
            }
           
        }

        public async Task<ResponseObject<bool>> Logout(Guid userId, string fcmToken)
        {
            var user = await _unitOfWork.User.GetById(userId, null, false);
            if (user is null)
            {
                return Success(false);
            }
            user.FcmToken.Remove(fcmToken);
            await _unitOfWork.User.SaveChangesAsync();
            return Success(true);
        }

        public async Task<ResponseObject<UserResponse>> Get(Guid id)
        {
            var query = new QueryHelper<User>()
            {
                Includes = new Expression<Func<User, object>>[] { x => x.Role },
            };
            User user = await _unitOfWork.User.GetById(id, query, false);
            if (user is null)
            {
                return NotFound<UserResponse>("User not found");
            }
            UserResponse userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = user.Role;
            return Success(userResponse);
        }

        public async Task<ResponseObject<UserResponse>> Update(Guid id, UpdateUserRequest userRequest)
        {
            User user = await _unitOfWork.User.GetById(id, null, false);
            
            if (user is null)
            {
                return NotFound<UserResponse>("User not found");
            }
            user.Email = userRequest.Email ?? "";
            user.FullName = userRequest.FullName ?? "";
            user.AvatarUrl = userRequest.AvatarUrl ?? "";
            user.Phone = userRequest.Phone ?? "";
            user.Birthday = userRequest.Birthday;

            await _unitOfWork.User.SaveChangesAsync();
            UserResponse userResponse = _mapper.Map<UserResponse>(user);
            return Success(userResponse);
        }

    }
}
