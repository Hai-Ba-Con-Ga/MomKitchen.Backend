using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Order
{
    public class UpdateOrderStatusReq
    {
        public OrderStatus OrderStatus { get; set; } = OrderStatus.UNPAID;
        public Guid OrderId { get; set; } = Guid.Empty;
    }
}
