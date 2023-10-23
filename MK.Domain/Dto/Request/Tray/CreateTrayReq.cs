using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Tray
{
    public class CreateTrayReq
    {
        [Required]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string ImgUrl { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid KitchenId { get; set; }
        
        public IEnumerable<Guid> DishIds { get; set; } = null!;
    }
}
