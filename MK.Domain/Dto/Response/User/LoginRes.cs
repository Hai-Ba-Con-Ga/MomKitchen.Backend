using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.User
{
    public class LoginRes
    {
        public bool IsFirstTime { get; set; }
        public string Token { get; set; } = null!;
        public UserRes User { get; set; } = null!;
    }
}
