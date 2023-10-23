using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Meal
{
    public class UpdateMealReq
    {
        public string Name { get; set; } = null;

        public decimal? Price { get; set; } = null;

        public DateTime? ServiceFrom { get; set; } = null;

        public DateTime? ServiceTo { get; set; } = null;

        public int? ServiceQuantity { get; set; } = null;

        public DateTime? CloseTime { get; set; } = null;
    }
}
