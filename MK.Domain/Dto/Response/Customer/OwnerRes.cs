using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Customer
{
    public class OwnerRes
    {
        public Guid OwnerId { get; set; }

        public string OwnerName { get; set; }

        public string OwnerAvatarUrl { get; set; }

        public string OwnerEmail { get; set; }
    }
}
