using MK.Domain.Dto.Request.Payment;
using MK.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v1/payment-type")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeService _paymentTypeService;
        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreatePaymentTypeReq req)
        {
            var result = await _paymentTypeService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{paymentTypeId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([Required] Guid paymentTypeId)
        {
            var result = await _paymentTypeService.Delete(paymentTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{paymentTypeId}")]
        [ProducesResponseType(typeof(PaymentType), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([Required] Guid paymentTypeId)
        {
            var result = await _paymentTypeService.GetById(paymentTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentTypeService.GetAll();
            return StatusCode((int)result.StatusCode, result);
        }

    }
}