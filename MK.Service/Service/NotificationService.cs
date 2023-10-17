using FirebaseAdmin.Messaging;
using MK.Application.Repository;
using MK.Domain.Common;
using MK.Domain.Dto.Request.Notification;
using MK.Domain.Dto.Response.Notification;
using MK.Domain.Entity;

namespace MK.Service.Service
{
    public class NotificationService : BaseService, INotificationService
    {

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<ResponseObject<NotificationResponse>> Create(CreateNotificationRequest notificationRequest)
        {
            try
            {
                var user = await _unitOfWork.User.GetById(notificationRequest.ReceiverId, null, false);
                if (user is null)
                {
                    return BadRequest<NotificationResponse>("User not found");
                }
                var notificationId = await _unitOfWork.Notification.CreateAsync(_mapper.Map<Domain.Entity.Notification>(notificationRequest), true);
                if (notificationId == Guid.Empty)
                {
                    return BadRequest<NotificationResponse>("Create notification failed");
                }
                var firebaseResponse = await SendNotificationMultiDeviceAsync(user.FcmToken, notificationRequest.Title, notificationRequest.Content);
                if (firebaseResponse == "0")
                {
                    return BadRequest<NotificationResponse>("Send notification failed");
                }

                return Success<NotificationResponse>(_mapper.Map<NotificationResponse>(notificationRequest));

            }
            catch (Exception ex)
            {
                return BadRequest<NotificationResponse>(ex.Message);
            }

        }

        public async Task<PaginationResponse<NotificationResponse>> GetAll(PaginationParameters paginationparam = null)
        {
            try
            {
                var query = new QueryHelper<Domain.Entity.Notification, NotificationResponse>()
                {
                    Include = i => i.Include(x => x.Receiver),
                    PaginationParams = paginationparam ??= new PaginationParameters(),
                };
                var notifications = await _unitOfWork.Notification.GetWithPagination(query);
                return Success(notifications);
            }
            catch (Exception ex)
            {
                return BadRequests<NotificationResponse>(ex.Message);
            }

        }

        public async Task<string> SendNotificationOneDeviceAsync(string fcmToken, string title, string content)
        {
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                   {
                       { "title", title },
                       { "content", content }
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
                Data = new Dictionary<string, string>()
                {
                       { "title", title },
                       { "content", content }
                   },
                Tokens = fcmTokens

            };
            BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return response.SuccessCount.ToString();
        }
    }
}
