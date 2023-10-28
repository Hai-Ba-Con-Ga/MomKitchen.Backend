using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Notification
{
    public class NotificationRes
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public NotificationType NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime SentTime { get; set; }
    }
}
