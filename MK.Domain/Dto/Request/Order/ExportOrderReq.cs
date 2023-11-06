using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Order
{
    public class ExportOrderReq
    {
        public string ApiEndpoint { get; set; } = null;
        public string EmailSender { get; set; } = null;
    }
}
