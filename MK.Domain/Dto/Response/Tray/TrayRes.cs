﻿using MK.Domain.Dto.Response.Dish;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Response.Dish;

namespace MK.Domain.Dto.Response
{
    public class TrayRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public IEnumerable<DishRes>? Dishes {get;set;}
    }
}
