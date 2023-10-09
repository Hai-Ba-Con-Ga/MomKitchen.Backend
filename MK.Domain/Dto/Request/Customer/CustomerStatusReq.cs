using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Enum;

namespace MK.Domain.Dto.Request.Customer
{
    public class CustomerStatusReq
    {
        public CustomerStatus Status { get; set; }
    }
}
