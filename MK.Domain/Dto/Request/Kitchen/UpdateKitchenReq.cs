using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Kitchen
{
    public class UpdateKitchenReq
    {
        public string Name { get; set; } = null;
        public string Address { get; set; } = null;
        public KitchenStatus? Status { get; set; } = null;
        public CreateLocationReq Location { get; set; } = null;
    }
}
