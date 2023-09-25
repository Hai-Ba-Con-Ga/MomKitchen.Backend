using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("favourite_kitchen")]
    public partial class FavouriteKitchen : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        public Guid KitchenId { get; set; }
        public virtual Kitchen Kitchen { get; set; } = null!;
    }
}
