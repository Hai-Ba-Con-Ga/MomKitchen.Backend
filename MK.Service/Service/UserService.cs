using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.User;
using MK.Domain.Dto.Response.User;
using MK.Domain.Entity;
using MK.Service.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MK.Application.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }


        public async Task<ResponseObject<UserResponse>> Create(CreateUserRequest userRequest)
        {
            try
            {
                var queryRole = new QueryHelper<Role>()
                {
                    Filter = x => x.Name.Equals(userRequest.RoleName),
                };
                var role = (await _unitOfWork.Role.Get(queryRole)).FirstOrDefault();
                if (role is null)
                {
                    return BadRequest<UserResponse>("Invalid role");
                }

                if (userRequest.Email is null && userRequest.Phone is null)
                {
                    return BadRequest<UserResponse>("Invalid token");
                }

                var query = new QueryHelper<User>()
                {
                    Filter = x => (x.Email == userRequest.Email || x.Phone == userRequest.Phone),
                    Includes = new Expression<Func<User, object>>[] { x => x.Role },
                };

                var user = (await _unitOfWork.User.Get(query, false)).FirstOrDefault();
                if (user != null)
                {
                    return BadRequest<UserResponse>("User already exists");
                }

                user = new User()
                {
                    Email = userRequest.Email,
                    FullName = userRequest.FullName,
                    AvatarUrl = userRequest.AvatarUrl,
                    Phone = userRequest.Phone,
                    Birthday = userRequest.Birthday,
                    FcmToken = new List<string>() { userRequest.FcmToken },
                    RoleId = role.Id,
                };
                var userId = await _unitOfWork.User.CreateAsync(user, true);
                if (userId == Guid.Empty)
                {
                    return BadRequest<UserResponse>("Create user failed");
                }
                var userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Role = role;

                return Success(userResponse);
            }
            catch (Exception e)
            {

                return BadRequest<UserResponse>(e.Message);
            }

        }

        public async Task<ResponseObject<UserResponse>> Update(Guid id, UpdateUserRequest userRequest)
        {
            try
            {
                User user = await _unitOfWork.User.GetById(id, null, false);
                if (user == null)
                {
                    return BadRequest<UserResponse>("User not found");
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
            catch (Exception e)
            {
                return BadRequest<UserResponse>(e.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid id)
        {
            try
            {
                var result = await _unitOfWork.User.SoftDeleteAsync(x => x.Id == id);
                return Success(result > 0);
            }
            catch (Exception e)
            {
                return BadRequest<bool>(e.Message);
            }
        }

        public async Task<ResponseObject<UserResponse>> GetById(Guid id)
        {
            try
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
            catch (Exception e)
            {
                return BadRequest<UserResponse>(e.Message);
            }
        }


        public async Task<PaginationResponse<UserResponse>> GetAll(PaginationParameters paginationparam = null)
        {
            try
            {
                var query = new QueryHelper<User, UserResponse>()
                {
                    PaginationParams = paginationparam ??= new PaginationParameters(),
                    Includes = new Expression<Func<User, object>>[] { x => x.Role },
                };
                var resultQuery = await _unitOfWork.User.GetWithPagination(query);
                return Success(resultQuery);
            }
            catch (Exception ex)
            {
                return BadRequests<UserResponse>(ex.Message);
            }

        }
        public async Task<ResponseObject<bool>> UpdateRole(Guid userId, string roleName)
        {
            var user = await _unitOfWork.User.GetById(userId, null, false);
            if (user is null)
            {
                return BadRequest<bool>("User not found");
            }
            var role = await _unitOfWork.Role.FirstOrDefaultAsync(x => x.Name.Equals(roleName));
            if (role is null)
            {
                return BadRequest<bool>("Role not found");
            }
            user.RoleId = role.Id;
            await _unitOfWork.User.SaveChangesAsync();
            return Success(true);
        }

    }
}
