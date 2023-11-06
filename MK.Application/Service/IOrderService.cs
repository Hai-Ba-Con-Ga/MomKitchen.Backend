using MK.Domain.Dto.Request.Order;
using MK.Domain.Dto.Response.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface IOrderService
    {
        Task<ResponseObject<OrderDetailRes>> GetOrderById(Guid orderId);
        Task<PagingResponse<OrderDetailRes>> GetAllOrder(PagingParameters pagingParam, GetOrderReq getOrderReq);
        Task<ResponseObject<Guid>> CreateOrder(CreateOrderReq orderReq);
        Task<ResponseObject<bool>> DeleteOrder(Guid orderId);
        Task<ResponseObject<bool>> UpdateOrderStatus(UpdateOrderStatusReq req);
    }
}
