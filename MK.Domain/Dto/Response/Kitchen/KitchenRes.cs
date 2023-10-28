using MK.Domain.Dto.Response.Customer;
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
        public int No { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string ImgUrl { get; set; }

        public int? NoOfDish { get; set; } = null;

        public int? NoOfTray { get; set; } = null;

        public int? NoOfMeal { get; set; } = null;

        public float? Rating { get; set; } = null;

        public KitchenStatus Status { get; set; }

        public LocationRes Location { get; set; } = null;

        public OwnerRes Owner { get; set; } = null;

        public GetAreaRes Area { get; set; } = null;
    }
}
