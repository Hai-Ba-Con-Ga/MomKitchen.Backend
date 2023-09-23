using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("notification")]
    public partial class Notification : BaseEntity
    {
        [Column("content")]
        [MaxLength(255)]
        public string Content { get; set; } = null!;

        [Column("title")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        public NotificationType NotificationType { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("Notifications")]
        public virtual User Receiver { get; set; } = null!;
    }
}
