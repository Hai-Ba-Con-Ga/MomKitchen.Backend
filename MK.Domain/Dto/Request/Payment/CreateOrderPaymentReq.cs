using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Enum;

namespace MK.Domain.Dto.Request.Payment
{
    public class CreateOrderPaymentReq
    {
        [Required]
        public decimal Amount { get; set; }

        public int LimitMonth { get; set; } = 1;
        //TODO: Add OrderId
        //public Guid OrderId { get; set; }
        public Guid PaymentTypeId { get; set; }
    }
}
