using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("customer")]
    public partial class Customer : BaseEntity
    {
        [Column("point_wallet")]
        public int PointWallet { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Customer")]
        public virtual User User { get; set; } = null!;

        public CustomerStatus Status { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<FavouriteKitchen> FavouriteKitchens { get; set; } = new List<FavouriteKitchen>();

        [InverseProperty("Customer")]
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        [InverseProperty("Customer")]
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        [InverseProperty("Customer")]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
