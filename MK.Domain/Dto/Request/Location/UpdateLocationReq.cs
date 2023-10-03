using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class UpdateLocationReq
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
