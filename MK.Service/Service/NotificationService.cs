using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Service
{
    public class NotificationService
    {
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
