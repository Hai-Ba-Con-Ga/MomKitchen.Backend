using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("tray")]
    public class Tray : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [DataType(DataType.Text)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string ImgUrl { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = null!;

        public virtual ICollection<Dish> Dishies { get; set; } = new List<Dish>();
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
