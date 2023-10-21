using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Dish
{
    public class DishDetailRes
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public DishStatus Status { get; set; }
        public Guid KitchenId { get; set; }
        public string KitchenName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
