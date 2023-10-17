using MK.Domain.Dto.Response.Customer;
using MK.Domain.Entity;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int NoOfDish { get; set; }

        public int NoOfTray { get; set; }

        public int NoOfMeal { get; set; }

        public float Rating { get; set; }

        public KitchenStatus Status { get; set; }

        public LocationRes Location { get; set; }

       public OwnerRes Owner { get; set; }

       public GetAreaRes Area { get; set; }
    }
}
