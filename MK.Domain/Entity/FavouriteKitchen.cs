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
        [ForeignKey("CustomerId")]
        [InverseProperty("FavouriteKitchens")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("KitchenId")]
        //[InverseProperty("FavouriteKitchens")]
        public virtual Kitchen Kitchen { get; set; } = null!;


    }
}
