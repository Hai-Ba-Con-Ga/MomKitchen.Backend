using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using MK.Domain.Dto.Request.Feedback;
using MK.Domain.Dto.Response.Feedback;

namespace MK.Service.Service
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<ResponseObject<Guid>> Create(CreateFeedbackReq req)
        {
            try
            {
                var newFeedback = _mapper.Map<Feedback>(req);
                var createResult = await _unitOfWork.Feedback.CreateAsync(newFeedback, isSaveChange: true);
                return Success(createResult);
            }
            catch (Exception e)
            {
                return BadRequest<Guid>(e.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete(Guid feedbackId)
        {
            try
            {
                var deleteResult = await _unitOfWork.Feedback.SoftDeleteAsync(t => t.Id == feedbackId);
                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }


        public async Task<ResponseObject<bool>> Update(Guid feedbackId, UpdateFeedbackReq req)
        {
            try
            {
                var queryHelper = new QueryHelper<Feedback>
                {
                    Include = t => t.Include(t => t.Customer)
                };
                var feedback = await _unitOfWork.Feedback.GetById(feedbackId, queryHelper, isAsNoTracking: false);

                feedback.Content = req.Content ?? feedback.Content;
                feedback.Rating = req.Rating > 0 ? req.Rating : feedback.Rating;
                feedback.ImgUrl = req.ImgUrl ?? feedback.ImgUrl;
                var updateResult = await _unitOfWork.Feedback.UpdateAsync(feedback, isSaveChange: true);
                return Success(updateResult > 0);

            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }

        }

        public async Task<ResponseObject<FeedbackRes>> GetById(Guid feedbackId)
        {
            try
            {
                var queryHelper = new QueryHelper<Feedback, FeedbackRes>()
                {
                    Selector = t => new FeedbackRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Content = t.Content,
                        Rating = t.Rating,
                        ImgUrl = t.ImgUrl,
                        Owner = new Domain.Dto.Response.Customer.OwnerRes()
                        {
                            OwnerId = t.CustomerId,
                            OwnerName = t.Customer.User.FullName,
                            OwnerAvatarUrl = t.Customer.User.AvatarUrl,
                            OwnerEmail = t.Customer.User.Email,
                        },
                        OrderId = t.OrderId,
                    },
                    Include = t => t.Include(t => t.Customer).
                    ThenInclude(t => t.User),
                };
                var feedback = await _unitOfWork.Feedback.GetById(feedbackId, queryHelper);
                return Success(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest<FeedbackRes>(ex.Message);
            }
        }

        public async Task<PagingResponse<FeedbackRes>> GetAll(PagingParameters pagingParam = null, string[] fields = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Feedback, FeedbackRes>()
                {
                    Selector = t => new FeedbackRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Content = t.Content,
                        Rating = t.Rating,
                        ImgUrl = t.ImgUrl,
                        Owner = new Domain.Dto.Response.Customer.OwnerRes()
                        {
                            OwnerId = t.CustomerId,
                            OwnerName = t.Customer.User.FullName,
                            OwnerAvatarUrl = t.Customer.User.AvatarUrl,
                            OwnerEmail = t.Customer.User.Email,
                        },
                        OrderId = t.OrderId,
                    },
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields,
                    Include = t => t.Include(t => t.Customer).
                    ThenInclude(t => t.User),
                };
                var feedbacks = await _unitOfWork.Feedback.GetWithPagination(queryHelper);
                return Success(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequests<FeedbackRes>(ex.Message);
            }
        }

        public async Task<PagingResponse<FeedbackRes>> GetFeedbacksByKitchenId(Guid kitchenId, PagingParameters pagingParam = null, string[] fields = null)
        {
            try
            {
                var queryHelper = new QueryHelper<Feedback, FeedbackRes>()
                {
                    Selector = t => new FeedbackRes
                    {
                        No = t.No,
                        Id = t.Id,
                        Content = t.Content,
                        Rating = t.Rating,
                        ImgUrl = t.ImgUrl,
                        Owner = new Domain.Dto.Response.Customer.OwnerRes()
                        {
                            OwnerId = t.CustomerId,
                            OwnerName = t.Customer.User.FullName,
                            OwnerAvatarUrl = t.Customer.User.AvatarUrl,
                            OwnerEmail = t.Customer.User.Email,
                        },
                        OrderId = t.OrderId,
                    },
                    Include = t => t.Include(t => t.Customer).ThenInclude(t => t.User),
                    PagingParams = pagingParam ??= new PagingParameters(),
                    OrderByFields = fields,
                    Filter = t => t.KitchenId == kitchenId,
                };
                var feedbacks = await _unitOfWork.Feedback.GetWithPagination(queryHelper);
                return Success(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequests<FeedbackRes>(ex.Message);
            }
        }

    }
}
