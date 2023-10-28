using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Notification
{
    public class CreateNotificationReq
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid ReceiverId { get; set; }
        public DateTime SentTime { get; set; }
    }
}
