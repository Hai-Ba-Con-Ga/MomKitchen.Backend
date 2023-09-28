using MK.API.Application.Repository;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public class UserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        
        public async Task<UserResponse> Create(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            await _userRepository.CreateAsync(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Update(Guid id, UserRequest userRequest)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user = _mapper.Map(userRequest, user);
            _userRepository.Update(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<int> Delete(Guid id)
        {
            return await _userRepository.SoftDeleteAsync(x => x.Id == id);
        }

        public async Task<UserResponse> GetById(Guid id)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return _mapper.Map<UserResponse>(user);
        }

        //TODO: add paging get all filter
        
    }
}
