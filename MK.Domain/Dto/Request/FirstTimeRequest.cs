using MapsterMapper;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class FirstTimeRequest 
    {
        public string FirebaseToken { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }

        public DateTime Birthday {  get; set; }

        public Role Role { get; set; }


    }
}
