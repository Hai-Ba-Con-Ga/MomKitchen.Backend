using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("order_payment")]
    public partial class OrderPayment : BaseEntity
    {

        public PaymentStatus Status { get; set; }
        [Required]
        public decimal amount { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Required]
        public Guid PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; } = null!;
    }
}
