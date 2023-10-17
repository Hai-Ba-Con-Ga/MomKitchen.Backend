using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.User
{
    public class LoginReq
    {
        public string IdToken { get; set; } = null!;
        public string? FcmToken { get; set; }
        public string RoleName { get; set; } = "Customer";

    }
}
