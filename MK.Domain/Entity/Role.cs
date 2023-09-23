using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("role")]
    public class Role : BaseEntity
    {
        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
