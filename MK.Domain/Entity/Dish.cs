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
    [Table("dish")]
    public partial class Dish : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("price")]
        public double Price { get; set; }

        [Column("image_url")]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = null!;

        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        public DishStatus Status { get; set; }

        //many to many with meal
        [InverseProperty("Dishes")]
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
