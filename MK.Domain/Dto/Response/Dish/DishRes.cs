using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Dish
{
    public class DishRes
    {
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
    }
}
