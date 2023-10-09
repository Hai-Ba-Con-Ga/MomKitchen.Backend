using MK.Domain.Dto.Request.Storage;
using MK.Domain.Dto.Response.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IStorageService
    {
        Task<ResponseObject<StorageRes>> UploadFile(StorageReq storageReq);
    }
}
