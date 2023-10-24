using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Order
{
    public class OrderRes
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Surcharge { get; set; }
        public OrderStatus Status { get; set; }
    }
}
