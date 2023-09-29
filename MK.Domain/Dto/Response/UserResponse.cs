using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string? FcmToken { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }

        public DateTime? Birthday {  get; set; }

        public string RoleName { get; set; }
    }
}
