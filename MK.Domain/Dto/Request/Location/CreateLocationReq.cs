using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Location
{
    public class CreateLocationReq
    {
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }
    }
}
