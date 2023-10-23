using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response
{
    public class MealRes
    {
        public Guid Id { get; set; }
        public int No { get; set;}
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime ServiceFrom { get; set; }

        public DateTime ServiceTo { get; set; }

        public int ServiceQuantity { get; set; }

        public DateTime close_time { get; set; }
    }
}
