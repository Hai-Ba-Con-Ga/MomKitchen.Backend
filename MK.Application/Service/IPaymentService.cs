using Microsoft.Extensions.Primitives;
using MK.Domain.Dto.Request.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(CreateOrderPaymentReq payment, string origin);
        Task<ResponseObject<OrderPayment>> GetById(Guid paymentId);
        Task<PagingResponse<OrderPayment>> GetAll(PagingParameters pagingParam = null);
        Task<ResponseObject<bool>> DeletePayment(Guid paymentId);
    }
}
