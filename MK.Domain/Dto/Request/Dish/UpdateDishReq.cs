using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto
{
    public class UpdateDishReq
    {
        public string Name { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
        public string? Description { get; set; }
    }
}
