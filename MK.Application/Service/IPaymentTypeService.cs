using MK.Domain.Dto.Request.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IPaymentTypeService
    {
        Task<ResponseObject<Guid>> Create(CreatePaymentTypeReq req);
        Task<ResponseObject<bool>> Delete (Guid paymentTypeId);
        Task<ResponseObject<PaymentType>> GetById(Guid paymentTypeId);
        // Task<IEnumerable<PaymentType>> GetAll();
    }
}