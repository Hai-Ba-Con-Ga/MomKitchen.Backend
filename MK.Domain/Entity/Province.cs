using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("province")]
    public partial class Province : BaseEntity
    {
        [Column("no")]
        [StringLength(10)]
        public string No { get; set; } = null!;

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [InverseProperty("Province")]
        public virtual ICollection<District> Districts { get; set; } = new List<District>();

        [InverseProperty("Province")]
        public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();

        [InverseProperty("Province")]
        public virtual ICollection<Kitchen> Kitchens { get; set; } = new List<Kitchen>();
    }
}
