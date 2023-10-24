using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Order;
using MK.Domain.Dto.Response.Order;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Function to get order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(ResponseObject<OrderDetailRes>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var orderDetail = await _orderService.GetOrderById(orderId);
            return StatusCode((int)orderDetail.StatusCode, orderDetail);
        }

        /// <summary>
        /// Function to get all order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseObject<OrderDetailRes>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrder(string keySearch, [FromQuery] PagingParameters pagingParam, [FromQuery] string[] fields)
        {
            var orderDetail = await _orderService.GetAllOrder(pagingParam, fields, keySearch);
            return StatusCode((int)orderDetail.StatusCode, orderDetail);
        }

        /// <summary>
        /// Function to create order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseObject<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrder(CreateOrderReq createReq)
        {
            var orderDetail = await _orderService.CreateOrder(createReq);
            return StatusCode((int)orderDetail.StatusCode, orderDetail);
        }
    }
}
