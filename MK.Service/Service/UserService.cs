using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using MK.Service.Service;

namespace MK.Application.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }


        public async Task<ResponseObject<UserResponse>> Create(UserRequest userRequest)
        {
            try
            {
                var user = _mapper.Map<User>(userRequest);
                user.Role = await _unitOfWork.Role.FirstOrDefaultAsync(x => x.Name.Equals(userRequest.RoleName));
                var userId = await _unitOfWork.User.CreateAsync(user, true);
                if (userId == Guid.Empty)
                {
                    return BadRequest<UserResponse>("Create user failed");
                }
                user.Id = userId;
                
                return Success(_mapper.Map<UserResponse>(user));
            }
            catch (Exception e)
            {

                return BadRequest<UserResponse>(e.Message);
            }
            
        }

        public async Task<ResponseObject<UserResponse>> Update(Guid id, UserRequest userRequest)
        {
            try
            {
                var query = new QueryHelper<User>()
                {
                    Filter = x => x.Id == id,
                    Includes = new Expression<Func<User, object>>[] { x => x.Role },
                };

                var user = (await _unitOfWork.User.Get(query)).FirstOrDefault();
                if (user == null)
                {
                    return BadRequest<UserResponse>("User not found");
                }
                user = _mapper.Map(userRequest, user);
                await _unitOfWork.User.SaveChangesAsync();
                return Success(_mapper.Map<UserResponse>(user));
            }catch(Exception e)
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
            }catch(Exception e)
            {
                return BadRequest<bool>(e.Message);
            }
        }

        public async Task<ResponseObject<UserResponse>> GetById(Guid id)
        {
            try
            {
                var query = new QueryHelper<User, UserResponse>()
                {
                    Filter = x => x.Id == id,
                };
                var result = (await _unitOfWork.User.Get(query)).FirstOrDefault();
                if (result == null)
                {
                    return BadRequest<UserResponse>("User not found");
                }
                
                return Success(result);
                

            }catch (Exception e)
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
        
    }
}
