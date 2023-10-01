using Autofac.Diagnostics;
using Microsoft.EntityFrameworkCore.Update;
using MK.Application.Repository;
using MK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        protected BaseService(IUnitOfWork unitOfWork,
         IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected ResponseObject<T> Success<T>(T data)
        {
            return new ResponseObject<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        protected PaginationResponse<T> Success<T>(PagedList<T> data) 
        {
            return new PaginationResponse<T>(data)
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        protected ResponseObject<T> BadRequest<T>(string message)
        {
            return new ResponseObject<T>
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        protected PaginationResponse<T> BadRequests<T>(string message)
        {
            return new PaginationResponse<T>
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        protected ResponseObject<T> NotFound<T>(string message)
        {
            return new ResponseObject<T>
            {
                Message = message,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        protected PaginationResponse<T> NotFounds<T>(string message)
        {
            return new PaginationResponse<T>
            {
                Message = message,
                StatusCode = HttpStatusCode.NotFound,
            };
        }
    }
}
