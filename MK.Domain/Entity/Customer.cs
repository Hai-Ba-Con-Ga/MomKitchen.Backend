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
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public CustomerStatus Status { get; set; } = CustomerStatus.ACTIVE;

        [InverseProperty("Customer")]
        public virtual ICollection<FavouriteKitchen> FavouriteKitchens { get; set; } = new List<FavouriteKitchen>();

        [InverseProperty("Customer")]
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        [InverseProperty("Customer")]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
