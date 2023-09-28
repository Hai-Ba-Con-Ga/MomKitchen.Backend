using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response
{
    public class LocationRes
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
