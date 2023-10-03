using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("location")]
    public class Location : BaseEntity
    {
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }

        public virtual Kitchen? Kitchen { get; set; }
    }
}
