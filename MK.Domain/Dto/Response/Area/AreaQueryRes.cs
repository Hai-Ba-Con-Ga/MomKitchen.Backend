using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Area
{
    public class AreaQueryRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid[] Boundaries { get; set; } = null!;
    }
}
