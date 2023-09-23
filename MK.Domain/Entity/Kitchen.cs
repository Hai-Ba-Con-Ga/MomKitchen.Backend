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

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public KitchenStatus Status { get; set; }

        [ForeignKey("OwnerId")]
        [InverseProperty("Kitchen")]
        public User Owner { get; set; } = null!;

        [ForeignKey("ProvinceId")]
        [InverseProperty("Kitchens")]
        public virtual Province Province { get; set; } = null!;

        [ForeignKey("DistrictId")]
        [InverseProperty("Kitchens")]
        public virtual District District { get; set; } = null!;

        [ForeignKey("WardId")]
        [InverseProperty("Kitchens")]
        public virtual Ward Ward { get; set; } = null!;

        [InverseProperty("Kitchen")]
        public virtual ICollection<FavouriteKitchen> FavoriteKitchens { get; set; } = new List<FavouriteKitchen>();

        [InverseProperty("Kitchen")]
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        [InverseProperty("Kitchen")]
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        [InverseProperty("Kitchens")]
        public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

        [InverseProperty("Kitchen")]
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();


    }
}
