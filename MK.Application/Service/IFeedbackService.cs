using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Request.Feedback;
using MK.Domain.Dto.Response.Feedback;

namespace MK.Application.Service
{
    public interface IFeedbackService
    {
        Task<ResponseObject<Guid>> Create(CreateFeedbackReq req);
        Task<ResponseObject<bool>> Delete(Guid feedbackId);
        Task<ResponseObject<bool>> Update(Guid feedbackId, UpdateFeedbackReq req);
        Task<ResponseObject<FeedbackRes>> GetById(Guid feedbackId);
        Task<PagingResponse<FeedbackRes>> GetAll(PagingParameters pagingParam = null, string[] fields = null);
        Task<PagingResponse<FeedbackRes>> GetFeedbacksByKitchenId(Guid kitchenId, PagingParameters pagingParam = null, string[] fields = null);

        

    }
}
