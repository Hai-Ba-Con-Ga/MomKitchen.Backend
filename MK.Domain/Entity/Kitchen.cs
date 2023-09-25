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
    public partial class Kitchen : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public KitchenStatus Status { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        [InverseProperty("Kitchen")]
        public virtual ICollection<FavouriteKitchen> FavoriteKitchens { get; set; } = new List<FavouriteKitchen>();

        [InverseProperty("Kitchen")]
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        [InverseProperty("Kitchen")]
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}
