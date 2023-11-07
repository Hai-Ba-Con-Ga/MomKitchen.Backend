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


        public async Task<ResponseObject<UserRes>> Create(CreateUserReq userRequest)
        {
            try
            {
                //1. Check role id
                var queryRole = new QueryHelper<Role>()
                {
                    Filter = x => x.Name.Equals(userRequest.RoleName),
                };

                var role = (await _unitOfWork.Role.Get(queryRole)).FirstOrDefault();
                if (role is null)
                {
                    return BadRequest<UserRes>("Invalid role");
                }

                //2. Check token
                if (userRequest.Email is null && userRequest.Phone is null)
                {
                    return BadRequest<UserRes>("Invalid token");
                }

                //3/ Check user exists
                var query = new QueryHelper<User>()
                {
                    Filter = x => (x.Email == userRequest.Email || x.Phone == userRequest.Phone),
                    Include = t => t.Include(i => i.Role),
                };

                var user = (await _unitOfWork.User.Get(query, false)).FirstOrDefault();
                if (user != null)
                {
                    return BadRequest<UserRes>("User already exists");
                }

                //Create user
                await _unitOfWork.BeginTransactionAsync();
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
                    return BadRequest<UserRes>("Create user failed");
                }

                if (role.Name == "Customer")
                {
                    var customerId = await _unitOfWork.Customer.CreateAsync(new Customer()
                    {
                        UserId = userId,
                    }, true);

                    if (customerId == Guid.Empty)
                    {
                        await _unitOfWork.RolebackTransactionAsync();
                        return BadRequest<UserRes>("Create customer failed");
                    }

                }

                await _unitOfWork.CommitTransactionAsync();

                var userResponse = _mapper.Map<UserRes>(user);
                userResponse.Role = role;

                return Success(userResponse);
            }
            catch (Exception e)
            {

                return BadRequest<UserRes>(e.Message);
            }

        }

        public async Task<ResponseObject<UserRes>> Update(Guid id, UpdateUserReq userRequest)
        {
            try
            {
                User user = await _unitOfWork.User.GetById(id, null, false);
                if (user == null)
                {
                    return BadRequest<UserRes>("User not found");
                }
                user.Email = userRequest.Email ?? "";
                user.FullName = userRequest.FullName ?? "";
                user.AvatarUrl = userRequest.AvatarUrl ?? "";
                user.Phone = userRequest.Phone ?? "";
                user.Birthday = userRequest.Birthday;
                await _unitOfWork.User.SaveChangesAsync();
                UserRes userResponse = _mapper.Map<UserRes>(user);
                return Success(userResponse);
            }
            catch (Exception e)
            {
                return BadRequest<UserRes>(e.Message);
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

        public async Task<ResponseObject<UserRes>> GetById(Guid id)
        {
            try
            {
                var query = new QueryHelper<User>()
                {
                    Include = t => t.Include(i => i.Role)
                                    .Include(i => i.Customer)
                                    .Include(i => i.Kitchen),

                };

                User user = await _unitOfWork.User.GetById(id, query, false);
                if (user is null)
                {
                    return NotFound<UserRes>("User not found");
                }

                UserRes userResponse = _mapper.Map<UserRes>(user);
                userResponse.Role = user.Role;

                if (user.Customer != null)
                {
                    userResponse.CustomerId = user.Customer.Id;
                }
                else if (user.Kitchen != null)
                {
                    userResponse.KitchenId = user.Kitchen.Id;
                }

                return Success(userResponse);


            }
            catch (Exception e)
            {
                return BadRequest<UserRes>(e.Message);
            }
        }

        public async Task<PagingResponse<UserRes>> GetAll(string roleName, string searchKey, PagingParameters paginationparam = null)
        {
            searchKey = searchKey ?? string.Empty;

            try
            {
                var query = new QueryHelper<User, UserRes>()
                {
                    Filter = x => x.Role.Name.Equals(roleName) &&
                                    (x.Email.Contains(searchKey)
                                    || x.Phone.Contains(searchKey)
                                    || x.FullName.Contains(searchKey)),
                    PagingParams = paginationparam ??= new PagingParameters(),
                    Include = t => t.Include(x => x.Role),
                };
                var resultQuery = await _unitOfWork.User.GetWithPagination(query);
                return Success(resultQuery);
            }
            catch (Exception ex)
            {
                return BadRequests<UserRes>(ex.Message);
            }

        }

        public async Task<ResponseObject<bool>> UpdateRole(Guid userId, string roleName)
        {
            var user = await _unitOfWork.User.GetById(userId, null, false);
            if (user is null)
            {
                return BadRequest<bool>("User not found");
            }
            var role = await _unitOfWork.Role.GetFirstOrDefaultAsync(x => x.Name.Equals(roleName));
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
