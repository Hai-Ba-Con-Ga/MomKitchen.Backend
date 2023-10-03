using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Area
{
    public class UpdateAreaReq
    {
        public UpdateLocationReq North { get; set; }
        public UpdateLocationReq South { get; set; }
        public UpdateLocationReq West { get; set; }
        public UpdateLocationReq East { get; set; }

    }
}
