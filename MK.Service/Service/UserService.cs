using MK.API.Application.Repository;
using MK.Domain.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<User> SignUpUserAsync(FirstTimeRequest firstTimeRequest)
        {
            User user = _mapper.Map<User>(firstTimeRequest);

            await _userRepository.CreateAsync(user);

            return user;

        }

        
    }
}
