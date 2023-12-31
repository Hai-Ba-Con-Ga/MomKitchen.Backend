﻿using MK.Domain.Dto.Request.Notification;
using MK.Domain.Dto.Request.Order;
using MK.Domain.Dto.Response.Order;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly INotificationService _notificationService;
        
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService) : base(unitOfWork, mapper)
        {
            _notificationService = notificationService;
        }
        public async Task<ResponseObject<OrderDetailRes>> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _unitOfWork.Order.GetById(orderId, new QueryHelper<Order, OrderDetailRes>()
                {
                    Selector = t => _mapper.Map<OrderDetailRes>(t),
                    Include = t => t.Include(x => x.Customer).Include(x => x.Meal).Include(x => x.Feedback)
                });

                return Success(order);
            }
            catch (Exception ex)
            {
                return BadRequest<OrderDetailRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<PagingResponse<OrderRes>> GetAllOrder(PagingParameters pagingParam, string[] fields, string keySearch)
        {
            try
            {
                var queryHelper = new QueryHelper<Order, OrderRes>()
                {
                    Selector = t => _mapper.Map<OrderRes>(t),
                    OrderByFields = fields,
                    Filter = t => t.No.ToString() == keySearch || t.Id.ToString() == keySearch || keySearch == null,
                    PagingParams = pagingParam ??= new PagingParameters(),
                };

                var getResult = await _unitOfWork.Order.GetWithPagination(queryHelper);

                return Success(getResult);
            }
            catch (Exception ex)
            {
                return BadRequests<OrderRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<Guid>> CreateOrder(CreateOrderReq orderReq)
        {
            try
            {
                decimal totalPrice = 0;

                if (!await _unitOfWork.Customer.IsExist(t => t.Id == orderReq.CustomerId))
                    return BadRequest<Guid>("Customer is not exist");

                var meal = await _unitOfWork.Meal.GetById(orderReq.MealId);

                if (meal == null)
                    return BadRequest<Guid>("Meal not found");

                totalPrice = meal.Price * orderReq.TotalQuantity;

                var order = new Order()
                {
                    MealId = orderReq.MealId,
                    Surcharge = totalPrice * 5 / 100,
                    TotalPrice = Decimal.ToDouble(totalPrice),
                    TotalQuantity = orderReq.TotalQuantity,
                    CustomerId = orderReq.CustomerId,
                };

                var orderPayment = new OrderPayment()
                {
                    OrderId = order.Id,
                    Amount = totalPrice,
                    PaymentTypeId = Guid.Parse("f2749ae0-9e3f-4e96-8baf-c5e6b29d330d"),
                    Status = PaymentStatus.Paid,
                };

                order.OrderPayments.Add(orderPayment);

                var createResult = await _unitOfWork.Order.CreateAsync(order, isSaveChange: true);
                var customer = await _unitOfWork.Customer.GetById(orderReq.CustomerId, null, false);
                if (customer == null)
                    return BadRequest<Guid>("Customer not found");

                var receiverId = customer.UserId;

                await _notificationService.Create(new CreateNotificationReq()
                {
                    ReceiverId = receiverId,
                    Title = "Order",
                    Content = "Your order has been created",
                });

                return Success(createResult);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.GetExceptionMessage());
            }
        }

        public async Task<ResponseObject<bool>> DeleteOrder(Guid orderId)
        {
            try
            {
                if (!await _unitOfWork.Order.IsExist(t => t.Id == orderId))
                    return BadRequest<bool>("Order not found");

                var deleteResult = await _unitOfWork.Order.SoftDeleteAsync(t => t.Id == orderId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }
    }
}
