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

        [Required]
        [StringLength(20)]
        [Column("password")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = null!;

        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [MaxLength(255)]
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [MaxLength(50)]
        [Column("fullname")]
        public string FullName { get; set; } = null!;

        public Guid RoleId { get; set; }
        public virtual Role? Role { get; set; } = null!;

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual Customer? Customer { get; set; }

        public virtual Kitchen? Kitchen { get; set; }

    }
}
