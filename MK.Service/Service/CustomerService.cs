using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Request.Customer;
using MK.Domain.Dto.Response.Customer;
using MK.Domain.Dto.Response.Order;

namespace MK.Service.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private const int RecentOrdersAmountGet = 5;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        //get customer all
        public async Task<PagingResponse<CustomerRes>> GetAll(PagingParameters pagingParam = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Customer, CustomerRes>()
                {
                    Selector = t => new CustomerRes
                    {
                        No = t.No,
                        Id = t.Id,
                        FullName = t.User.FullName,
                        Email = t.User.Email,
                        Phone = t.User.Phone,
                        Birthday = t.User.Birthday,
                        AvatarUrl = t.User.AvatarUrl,
                        Status = t.Status,
                        UserId = t.UserId,
                        OrderQuantity = t.Orders.Count,
                        SpentMoney = t.Orders.Sum(o => o.TotalPrice),
                        RecentOrders  = t.Orders.OrderByDescending(o => o.CreatedDate).Select(o => _mapper.Map<OrderRes>(o))
                    },
                    Include = i => i.Include(x => x.User)
                                    .Include(x => x.Orders)
                                    .Include(x => x.Feedbacks)
                                    .Include(x => x.FavouriteKitchens),
                    PagingParams = pagingParam ??= new PagingParameters()
                };
                var customer = await _unitOfWork.Customer.GetWithPagination(queryHelper);
                
                return Success(customer);
            }
            catch (Exception ex)
            {
                return BadRequests<CustomerRes>(ex.Message);
            }
        }
        //get customer by id
        public async Task<ResponseObject<CustomerRes>> GetById(Guid customerId)
        {
            try
            {
                var queryHelper = new QueryHelper<Customer, CustomerRes>()
                {
                    Selector = t => new CustomerRes
                    {
                        No = t.No,
                        Id = t.Id,
                        FullName = t.User.FullName,
                        Email = t.User.Email,
                        Phone = t.User.Phone,
                        Birthday = t.User.Birthday,
                        AvatarUrl = t.User.AvatarUrl,
                        Status = t.Status,
                        UserId = t.UserId,
                        OrderQuantity = t.Orders.Count,
                        SpentMoney = t.Orders.Sum(o => o.TotalPrice),
                        RecentOrders  = t.Orders.OrderByDescending(o => o.CreatedDate).Take(RecentOrdersAmountGet).Select(o => _mapper.Map<OrderRes>(o))
                    },
                    Include = t => t.Include(x => x.User)
                                    .Include(x => x.Orders)
                                    .Include(x => x.Feedbacks)
                                    .Include(x => x.FavouriteKitchens)
                };

                var customer = await _unitOfWork.Customer.GetById(customerId, queryHelper);
                return Success(customer);
            }
            catch (Exception ex)
            {
                return BadRequest<CustomerRes>(ex.Message);
            }
        }
        //ban customer
        public async Task<ResponseObject<bool>> Update(Guid kitchenId, CustomerStatusReq req)
        {
            try
            {
                var customer = await _unitOfWork.Customer.GetById(kitchenId);
                if (customer == null)
                {
                    return BadRequest<bool>("Customer not found");
                }
                customer.Status = req.Status;

                var updateResult = await _unitOfWork.Customer.UpdateAsync(customer, isSaveChange: true);
                return Success(updateResult > 0);
            }
            catch (Exception e)
            {
                return BadRequest<bool>(e.Message);
            }

        }

        public async Task<ResponseObject<bool>> Delete(Guid customerId)
        {
            try
            {
                if (!await _unitOfWork.Customer.IsExist(t => t.Id == customerId))
                {
                    return BadRequest<bool>("Customer not found");
                }

                var deleteResult = await _unitOfWork.Customer.SoftDeleteAsync(t => t.Id == customerId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.GetExceptionMessage());
            }
        }
    }
}
