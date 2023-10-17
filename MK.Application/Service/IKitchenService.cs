﻿using MK.Domain.Dto.Request.Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IKitchenService
    {
        Task<ResponseObject<Guid>> Create(CreateKitchenReq req);
        Task<ResponseObject<bool>> Delete(Guid kitchenId);
        Task<ResponseObject<bool>> Update(Guid kitchenId, UpdateKitchenReq req);
        Task<ResponseObject<KitchenRes>> GetById(Guid kitchenId);
        Task<PagingResponse<KitchenRes>> GetAll(PagingParameters pagingParam = null);
    }
}
