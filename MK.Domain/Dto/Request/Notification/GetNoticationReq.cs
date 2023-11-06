using MK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Notification
{
    public class GetNoticationReq : GetRequestBase
    {
        public Guid? UserId { get; set; } = null;
    }
}
