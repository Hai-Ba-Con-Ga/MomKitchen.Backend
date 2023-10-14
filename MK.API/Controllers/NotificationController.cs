using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Notification;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        /// <summary>
        /// Fucntion to create new notification
        /// </summary>
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationRequest notificationRequest)
        {
            var result = await _notificationService.Create(notificationRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Fucntion to get all notification
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters paginationParameters)
        {
            var result = await _notificationService.GetAll(paginationParameters);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
