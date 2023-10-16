using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.FavouriteKitchen
{
    public class FavouriteKitchenRes
    {
        public Guid CustomerId { get; set; }
        public Guid KitchenId { get; set; }
        public string KitchenName { get; set; } 
    }
}
