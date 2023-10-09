
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Kitchen
{
    public class CreateKitchenReq
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        public KitchenStatus Status { get; set; } = KitchenStatus.ACTIVE;
        [Required]
        public CreateLocationReq Location { get; set; } = null!;
        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public Guid AreaId { get; set; }
    }
}
