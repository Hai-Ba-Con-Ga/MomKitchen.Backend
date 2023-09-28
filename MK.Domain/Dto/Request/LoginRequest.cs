using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class LoginRequest
    {
        public string IdToken { get; set; }
        public string FcmToken { get; set; }

    }
}
