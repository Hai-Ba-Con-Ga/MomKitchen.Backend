using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response
{
    public class GetAreaRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<LocationRes> Boundaries { get; set; }

    }
}
