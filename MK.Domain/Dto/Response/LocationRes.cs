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
        public double Lat { get; set; }
        public double Lng { get; set; }

        public Kitchen? Kitchen { get; set; }

        public Area? AreaAsNorth { get; set; }
        public Area? AreaAsSouth { get; set; }
        public Area? AreaAsWest { get; set; }
        public Area? AreaAsEast { get; set; }
    }
}
