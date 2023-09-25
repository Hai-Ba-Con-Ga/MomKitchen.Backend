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
    [Table("kitchen")]
    public partial class Kitchen : BaseEntity
    {
        [StringLength(150)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        [Required]
        public string Address { get; set; } = null!;

        public KitchenStatus Status { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        [Required]
        public Guid AreaId { get; set; }
        public Area Area { get; set; } = null!;

        public virtual ICollection<FavouriteKitchen> FavoriteKitchens { get; set; } = new List<FavouriteKitchen>();
        public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
        public virtual ICollection<Tray> Trays { get; set; } = new List<Tray>();
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
