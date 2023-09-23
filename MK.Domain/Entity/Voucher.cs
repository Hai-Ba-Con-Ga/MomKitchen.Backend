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
    [Table("voucher")]
    public partial class Voucher : BaseEntity
    {

        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; } = null!;

        [Column("discount")]
        public double Discount { get; set; }

        public VoucherStatus Status { get; set; }

        [ForeignKey("PromotionId")]
        [InverseProperty("Vouchers")]
        public Promotion Promotion { get; set; } = null!;

        [InverseProperty("Voucher")]
        public virtual ICollection<Order> Orders { get; set; } = null!;



    }
}
