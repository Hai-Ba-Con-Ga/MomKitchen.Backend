using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("ward")]
    public partial class Ward : BaseEntity
    {
        public string Name { get; set; } = null!;

        [ForeignKey("DistrictId")]
        [InverseProperty("Wards")]
        public virtual District District { get; set; } = null!;

        [ForeignKey("ProvinceId")]
        [InverseProperty("Wards")]
        public virtual Province Province { get; set; } = null!;

        [InverseProperty("Ward")]
        public virtual ICollection<Kitchen> Kitchens { get; set; } = new List<Kitchen>();
    }
}
