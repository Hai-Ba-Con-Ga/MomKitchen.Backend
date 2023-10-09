using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Request.Customer;
using MK.Domain.Dto.Response.Customer;

namespace MK.Service.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        //get customer all
        public async Task<PaginationResponse<CustomerRes>> GetAll(PaginationParameters pagingParam = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Customer, CustomerRes>()
                {
                    Selector = t => new CustomerRes
                    {
                        Id = t.Id,
                        FullName = t.User.FullName,
                        Email = t.User.Email,
                        Phone = t.User.Phone,
                        Birthday = t.User.Birthday,
                        AvatarUrl = t.User.AvatarUrl,
                        Status = t.Status
                    },
                    Includes = new Expression<Func<Customer, object>>[]{
                        t => t.User,
                        t => t.Orders,
                        t => t.Feedbacks,
                        t => t.FavouriteKitchens
                    },
                    PaginationParams = pagingParam ??= new PaginationParameters()
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
                        Id = t.Id,
                        FullName = t.User.FullName,
                        Email = t.User.Email,
                        Phone = t.User.Phone,
                        Birthday = t.User.Birthday,
                        AvatarUrl = t.User.AvatarUrl,
                        Status = t.Status
                    },
                    Includes = new Expression<Func<Customer, object>>[]{
                        t => t.User,
                        t => t.Orders,
                        t => t.Feedbacks,
                        t => t.FavouriteKitchens
                    }
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
    }
}
