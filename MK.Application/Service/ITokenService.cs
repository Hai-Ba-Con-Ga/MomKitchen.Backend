using MK.Domain.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface ITokenService
    {
        string GetToken(UserResponse user);
        IEnumerable<Claim> DecodeAndValidateToken(string token);
    }
}
