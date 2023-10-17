using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.User
{
    public class UserRes
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }

        public DateTime? Birthday { get; set; }

        private string? _roleName;
        public Role Role
        {
            set
            {
                _roleName = value.Name;
            }
        }
        public string RoleName
        {
            get
            {
                return _roleName;
            }
        }
    }
}
