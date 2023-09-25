using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("meal")]
    public partial class Meal : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("service_from")]
        public DateTime ServiceFrom { get; set; }

        [Column("service_to")]
        public DateTime ServiceTo { get; set; }

        [Column("service_quantity")]
        [Range(1, int.MaxValue)]
        public int ServiceQuantity { get; set; }

        [Required]
        public Guid TrayId { get; set; }
        public Tray Tray { get; set; } = null!;

        [Required]
        public Guid KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = null!;

        public virtual ICollection<Tray> Trays { get; set; } = new List<Tray>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
       
    }
}
