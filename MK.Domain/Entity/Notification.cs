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
        public string Content { get; set; } = null!;

        [Column("title")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        public NotificationType NotificationType { get; set; }

        [Required]
        public DateTime SentTime { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }
        public virtual User Receiver { get; set; } = null!;
    }
}
