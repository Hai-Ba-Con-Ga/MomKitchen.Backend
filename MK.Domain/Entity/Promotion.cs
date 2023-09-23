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
    [Table("promotion")]
    public partial class Promotion : BaseEntity
    {

        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("amount")]
        public int Amount { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }


        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        public PromotionStatus Status { get; set; }

        [InverseProperty("Promotions")]
        public virtual ICollection<Kitchen> Kitchens { get; set; } = null!;

        [InverseProperty("Promotion")]
        public virtual ICollection<Voucher> Vouchers { get; set; } = null!;
    }
}
