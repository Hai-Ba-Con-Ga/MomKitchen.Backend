using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Storage
{
    public class StorageReq
    {
        //upload file
        public IFormFile File { get; set; }
        
    }
}
