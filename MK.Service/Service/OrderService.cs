using MK.Application.Cache;
using MK.Domain.Dto.Request.Notification;
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

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ICacheManager cacheManager,
            INotificationService notificationService) : base(unitOfWork, mapper, cacheManager)
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
                    Include = t => t.Include(x => x.Customer)
                                        .ThenInclude(x => x.User)
                                    .Include(x => x.Meal)
                                        .ThenInclude(x => x.Tray)
                                            .ThenInclude(x => x.Dishies)
                                    .Include(x => x.Meal)
                                        .ThenInclude(x => x.Kitchen)
                                    .Include(x => x.Feedback)
                });

                return Success(order);
            }
            catch (Exception ex)
            {
                return BadRequest<OrderDetailRes>(ex.GetExceptionMessage());
            }
        }

        public async Task<PagingResponse<OrderDetailRes>> GetAllOrder(
            PagingParameters pagingParam,
            GetOrderReq getOrderReq
        )
        {
            try
            {
                getOrderReq.OrderBy ??= new string[] { };
                if (!getOrderReq.OrderBy.Contains("CreatedDate:desc"))
                {
                    getOrderReq.OrderBy = getOrderReq.OrderBy.Append("CreatedDate:desc").ToArray();
                }

                var queryHelper = new QueryHelper<Order, OrderDetailRes>()
                {
                    PagingParams = pagingParam ??= new PagingParameters(),
                    Selector = t => _mapper.Map<OrderDetailRes>(t),
                    OrderByFields = getOrderReq.OrderBy,
                    Include = t => t.Include(x => x.Customer)
                                        .ThenInclude(x => x.User)
                                    .Include(x => x.Meal)
                                        .ThenInclude(x => x.Tray)
                                            .ThenInclude(x => x.Dishies)
                                    .Include(x => x.Meal)
                                        .ThenInclude(x => x.Kitchen)
                                    .Include(x => x.Feedback),
                    Filter = t => (getOrderReq.KeySearch == null
                                        || t.No.ToString() == getOrderReq.KeySearch
                                        || t.Id.ToString() == getOrderReq.KeySearch)
                                && (t.CreatedDate.Date >= getOrderReq.FromDate && t.CreatedDate <= getOrderReq.ToDate)
                                && (getOrderReq.OrderStatus == null || t.Status == getOrderReq.OrderStatus)
                                && (getOrderReq.KitchenId == null || getOrderReq.KitchenId == t.Meal.KitchenId)
                                && (getOrderReq.OwnerId == null || getOrderReq.OwnerId == t.CustomerId)
                };

                var getResult = await _unitOfWork.Order.GetWithPagination(queryHelper);

                return Success(getResult);
            }
            catch (Exception ex)
            {
                return BadRequests<OrderDetailRes>(ex.GetExceptionMessage());
            }
        }

        //public Task<ResponseObject<OrderDetailRes>> GetOrdersByKitchenId(
        //    Guid kitchenId,
        //    PagingParameters pagingParam,
        //    GetOrderReq getOrderReq)
        //{
        //    try
        //    {
        //        var queryHelper = new QueryHelper<Order, OrderDetailRes>()
        //        {
        //            PagingParams = pagingParam ??= new PagingParameters(),
        //            Selector = t => _mapper.Map<OrderDetailRes>(t),
        //            OrderByFields = getOrderReq.OrderBy,
        //            Include = t => t.Include(x => x.Customer)
        //                                .ThenInclude(x => x.User)
        //                            .Include(x => x.Meal)
        //                                .ThenInclude(x => x.Tray)
        //                                    .ThenInclude(x => x.Dishies)
        //                            .Include(x => x.Meal)
        //                                .ThenInclude(x => x.Kitchen)
        //                            .Include(x => x.Feedback),
        //            Filter = t => (getOrderReq.KeySearch == null
        //                                || t.No.ToString() == getOrderReq.KeySearch
        //                                || t.Id.ToString() == getOrderReq.KeySearch)
        //                        && (t.CreatedDate.Date >= getOrderReq.FromDate && t.CreatedDate <= getOrderReq.ToDate)
        //                        && (getOrderReq.OrderStatus == null || t.Status == getOrderReq.OrderStatus)
        //        };

        //        var getResult = await _unitOfWork.Order.GetWithPagination(queryHelper);

        //        return Success(getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequests<OrderDetailRes>(ex.GetExceptionMessage());
        //    }
        //}

        public async Task<ResponseObject<Guid>> CreateOrder(CreateOrderReq orderReq)
        {
            try
            {
                decimal totalPrice = 0;

                var customer = await _unitOfWork.Customer.GetFirstOrDefault(new QueryHelper<Customer>()
                { Filter = t => t.UserId == orderReq.UserId }, false);

                if (customer == null)
                    return BadRequest<Guid>("Customer not found");

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
                    CustomerId = customer.Id,
                    Status = OrderStatus.UNPAID
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

                //Push notification
                await _notificationService.Create(new CreateNotificationReq()
                {
                    ReceiverId = customer.UserId,
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

        public async Task<ResponseObject<bool>> UpdateOrderStatus(UpdateOrderStatusReq req)
        {
            try
            {
                if (!await _unitOfWork.Order.IsExist(t => t.Id == req.OrderId))
                    return BadRequest<bool>("Order not found");

                var order = await _unitOfWork.Order.GetById(req.OrderId, isAsNoTracking: false);

                order.Status = req.OrderStatus;

                var result = await _unitOfWork.Order.UpdateAsync(order, isSaveChange: true);

                return Success(result > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
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
