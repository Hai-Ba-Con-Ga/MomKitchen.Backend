using MK.Domain.Entity;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response
{
    public class KitchenRes
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public KitchenStatus Status { get; set; }

        public LocationRes Location { get; set; }

        public Guid OwnerId { get; set; }

        public string OwnerName { get; set; }

        public Guid AreaId { get; set; }
        public string AreaName { get; set; } = null!;

        public int QuantiyMeal { get; set; }
        public int QuantiyDish { get; set; }
        public int QuuantiyTray { get; set; }

    }
}
