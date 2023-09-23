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
        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [ForeignKey("OrderId")]
        [InverseProperty("OrderPayment")]
        public Order Order { get; set; } = null!;

        public PaymentStatus Status { get; set; }
    }
}
