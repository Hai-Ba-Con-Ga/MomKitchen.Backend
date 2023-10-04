using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class UpdateAreaReq
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public Dictionary<Guid, UpdateLocationReq> UpdateData { get; set; } = null!;
    }
}
