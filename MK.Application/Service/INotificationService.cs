using MK.Domain.Dto.Request.Notification;
using MK.Domain.Dto.Response.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public interface INotificationService
    {
        Task<ResponseObject<NotificationResponse>> Create(CreateNotificationRequest notificationRequest);
        Task<PaginationResponse<NotificationResponse>> GetAll(PaginationParameters paginationparam = null);
        Task<string> SendNotificationOneDeviceAsync(string fcmToken, string title, string content);
        Task<string> SendNotificationMultiDeviceAsync(List<string> fcmTokens, string title, string content);
    }
}
