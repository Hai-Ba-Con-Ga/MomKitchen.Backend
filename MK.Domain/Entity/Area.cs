using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("area")]
    public partial class Area : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        //many to many with meal

        [InverseProperty("Areas")]
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    }
}
