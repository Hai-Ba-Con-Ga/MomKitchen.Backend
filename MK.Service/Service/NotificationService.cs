using FirebaseAdmin.Messaging;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.Notification;
using MK.Domain.Dto.Request.Order;
using MK.Domain.Dto.Response.Notification;
using MK.Domain.Entity;

namespace MK.Service.Service
{
    public class NotificationService : BaseService, INotificationService
    {

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<ResponseObject<NotificationRes>> Create(CreateNotificationReq notificationRequest)
        {
            try
            {
                var user = await _unitOfWork.User.GetById(notificationRequest.ReceiverId, null, false);
                if (user is null)
                {
                    return BadRequest<NotificationRes>("User not found");
                }
                var notificationId = await _unitOfWork.Notification.CreateAsync(_mapper.Map<Domain.Entity.Notification>(notificationRequest), true);
                if (notificationId == Guid.Empty)
                {
                    return BadRequest<NotificationRes>("Create notification failed");
                }
                var firebaseResponse = await SendNotificationMultiDeviceAsync(user.FcmToken, notificationRequest.Title, notificationRequest.Content);
                if (firebaseResponse == "0")
                {
                    return BadRequest<NotificationRes>("Send notification failed");
                }

                return Success<NotificationRes>(_mapper.Map<NotificationRes>(notificationRequest));

            }
            catch (Exception ex)
            {
                return BadRequest<NotificationRes>(ex.Message);
            }

        }

        public async Task<PagingResponse<NotificationRes>> GetAll(GetNoticationReq getReq, PagingParameters paginationparam = null)
        {
            try
            {
                var query = new QueryHelper<Domain.Entity.Notification, NotificationRes>()
                {
                    Include = i => i.Include(x => x.Receiver),
                    PagingParams = paginationparam ??= new PagingParameters(),
                    OrderByFields = getReq.OrderBy,
                    Filter = t => (getReq.KeySearch == null
                                        || t.No.ToString() == getReq.KeySearch
                                        || t.Id.ToString() == getReq.KeySearch)
                                && (t.CreatedDate.Date >= getReq.FromDate && t.CreatedDate <= getReq.ToDate)
                                && (getReq.UserId == null || t.ReceiverId == getReq.UserId)
                };
                var notifications = await _unitOfWork.Notification.GetWithPagination(query);
                return Success(notifications);
            }
            catch (Exception ex)
            {
                return BadRequests<NotificationRes>(ex.Message);
            }

        }

        public async Task<string> SendNotificationOneDeviceAsync(string fcmToken, string title, string content)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = content
                },
                Token = fcmToken

            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

        public async Task<string> SendNotificationMultiDeviceAsync(List<string> fcmTokens, string title, string content)
        {
            var message = new MulticastMessage()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = content
                },
                Tokens = fcmTokens

            };
            BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return response.SuccessCount.ToString();
        }
    }
}
