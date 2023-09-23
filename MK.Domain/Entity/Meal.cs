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
        public double Price { get; set; }

        [Column("service_from")]
        public DateTime ServiceFrom { get; set; }

        [Column("service_to")]
        public DateTime ServiceTo { get; set; }

        //many to many with area
        [InverseProperty("Meals")]
        public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

        //many to many with dish
        [InverseProperty("Meals")]
        public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

        //many to one with kitchen
        [ForeignKey("KitchenId")]
        [InverseProperty("Meals")]
        public Kitchen Kitchen { get; set; } = null!;
    }
}
