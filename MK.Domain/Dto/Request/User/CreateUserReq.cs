using MapsterMapper;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.User
{
    public class CreateUserReq
    {
        public string? FcmToken { get; set; } 
        public string Email { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Phone { get; set; } = null!; 
        public string RoleName { get; set; } = "Customer";
        public DateTime? Birthday { get; set; }
    }
}
