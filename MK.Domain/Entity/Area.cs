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
        [StringLength(150)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid NorthId { get; set; }
        public Location North { get; set; } = null!;

        [Required]
        public Guid SouthId { get; set; }
        public Location South { get; set; } = null!;

        [Required]
        public Guid EastId { get; set; }
        public Location East { get; set; } = null!;

        [Required]
        public Guid WestId { get; set; }
        public Location West { get; set; } = null!;

        public virtual ICollection<Kitchen> Kitchens { get; set; } = new List<Kitchen>();
    }
}
