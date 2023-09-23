using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("user")]
    public partial class User : BaseEntity
    {
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        [Column("password")]
        public string Password { get; set; } = null!;

        [StringLength(20)]
        public string Phone { get; set; } = null!;

        [DataType(DataType.DateTime)]
        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [MaxLength(255)]
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [MaxLength(50)]
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual Role? Role { get; set; } = null!;

        [InverseProperty("Receiver")]
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [InverseProperty("User")]
        public virtual Customer? Customer { get; set; }

        [InverseProperty("Owner")]
        public virtual ICollection<Kitchen> Kitchen { get; set; } = new List<Kitchen>();

    }
}
