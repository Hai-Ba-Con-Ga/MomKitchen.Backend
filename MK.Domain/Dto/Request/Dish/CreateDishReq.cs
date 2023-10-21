using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MK.Domain.Dto.Request
{
    public class CreateDishReq
    {
        [Required]
        public Guid KitchenId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public DishStatus Status { get; set; } = DishStatus.Active;
    }
}
