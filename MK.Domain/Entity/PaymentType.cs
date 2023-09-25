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
    [Table("payment_type")]
    public class PaymentType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Provider { get; set; } = null!;
        [MaxLength(150)]
        public string Name { get; set; } = null!;
        [DataType(DataType.Text)]
        public string Description { get; set; } = null!;

        public PaymentTypeStatus Status { get; set; } = PaymentTypeStatus.Active;


        public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();
    }
}
