using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Common
{
    public class GetByUserReq
    {
        public Guid? KitchenId { get; set; } = null;
        public Guid? CustomerId { get; set; } = null;
    }
}
