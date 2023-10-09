using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Request.Customer;
using MK.Domain.Dto.Response.Customer;

namespace MK.Application.Service
{
    public interface ICustomerService
    {
        Task<PaginationResponse<CustomerRes>> GetAll(PaginationParameters pagingParam = null);
        Task<ResponseObject<CustomerRes>> GetById(Guid customerId);
        Task<ResponseObject<bool>> Update(Guid kitchenId, CustomerStatusReq req);
        
    }
}
