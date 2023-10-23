using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Tray
{
    public class UpdateTrayReq
    {
        public string Name { get; set; }

        public string Description { get; set; } 

        [MaxLength(255)]
        public string ImgUrl { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<Guid> DishIds { get; set; }
    }
}
