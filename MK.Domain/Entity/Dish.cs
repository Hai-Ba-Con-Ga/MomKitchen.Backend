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

        [Column("image_url")]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        public DishStatus Status { get; set; }

        [Required]
        public Guid KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = null!;

        public virtual ICollection<Tray> Trays { get; set; } = new List<Tray>();
    }
}
