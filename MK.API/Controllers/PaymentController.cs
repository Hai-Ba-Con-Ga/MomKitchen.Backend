using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MK.Domain.Dto.Request.Payment;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateOrderPaymentReq req)
        {
            var origin = "https://mamakitchen.tech";
            var result = await _paymentService.Create(req, origin);
            return StatusCode(200, result);
        }
        [HttpGet("{paymentId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid paymentId)
        {
            var result = await _paymentService.GetById(paymentId);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("{paymentId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid paymentId)
        {
            var result = await _paymentService.DeletePayment(paymentId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("callback-vnpay")]
        public async Task<IActionResult> CallbackVnPay()
        {
            var queryDictionary = QueryHelpers.ParseQuery(Request.QueryString.Value);
            await _paymentService.ProcessCallback(queryDictionary);
            string frontendUrlCallBack = AppConfig.VnpayConfig.FrontendCallBack;

            string url = QueryHelpers.AddQueryString(frontendUrlCallBack, queryDictionary);
            return Redirect($"https://www.mamakitchen.tech/{url}");
        }


    }
}
