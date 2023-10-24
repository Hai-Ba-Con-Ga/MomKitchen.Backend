using MK.Domain.Dto.Request.Payment;
using MK.Domain.Entity;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Order
{
    public class CreateOrderReq
    {
        public int TotalQuantity { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid MealId { get; set; }

        public ICollection<CreateOrderPaymentReq> OrderPayments { get; set; } = new List<CreateOrderPaymentReq>();
    }
}
