using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Response.Tray;

namespace MK.Domain.Dto.Response.Meal
{
    public class MealDetailRes
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ServiceFrom { get; set; }
        public DateTime ServiceTo { get; set; }
        public int ServiceQuantity { get; set; }
        public DateTime CloseTime { get; set; }
        public TrayDetailRes Tray { get; set; } = null;
        public KitchenRes Kitchen { get; set; } = null;
    }
}
