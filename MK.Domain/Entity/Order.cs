using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("order")]
    public partial class Order : BaseEntity
    {
        public double TotalPrice { get; set; }

        [ForeignKey("VoucherId")]
        [InverseProperty("Orders")]
        public Voucher? Voucher { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public virtual Customer Customer { get; set; } = null!;

        public OrderStatus Status { get; set; }

        [InverseProperty("Order")]
        public OrderPayment? OrderPayment { get; set; }
       


    }
}
