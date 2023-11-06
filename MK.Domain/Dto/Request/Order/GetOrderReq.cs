using MK.Domain.Common;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Order
{
    public class GetOrderReq : GetRequestBase
    {
        public OrderStatus? OrderStatus { get; set; } = null;
        public Guid? KitchenId { get; set; } = null;
        public Guid? OwnerId { get; set; } = null;

    }
}
