using MK.Domain.Dto.Response.Dish;
using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Tray
{
    public class TrayDetailRes
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public Guid KitchenId { get; set; }
        public string KitchenName { get; set; }
        public virtual IEnumerable<DishRes>? Dishies { get; set; } = null;
        public DateTime CreatedDate { get; set; }
    }
}
