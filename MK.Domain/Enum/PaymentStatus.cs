using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Enum
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Cancelled = 3,
        Failed = 4
    }
}
