using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("district")]
    public partial class District : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [ForeignKey("ProvinceId")]
        [InverseProperty("Districts")]
        public Province Province { get; set; } = null!;

        [InverseProperty("District")]
        public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();

        [InverseProperty("District")]
        public virtual ICollection<Kitchen> Kitchens { get; set; } = new List<Kitchen>();

    }
}
