using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Customer;
using MK.Domain.Dto.Response.Customer;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CustomerController :  ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        /// <summary>
        /// Function to get all Customer with paging
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(typeof(PagingResponse<CustomerRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters pagingParam = null)
        {
            var result = await _customerService.GetAll(pagingParam);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get Customer by Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid customerId)
        {
            var result = await _customerService.GetById(customerId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update Customer status
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{customerId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid customerId, CustomerStatusReq req)
        {
            var result = await _customerService.Update(customerId, req);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
