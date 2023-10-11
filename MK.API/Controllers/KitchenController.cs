using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Kitchen;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        /// <summary>
        /// Function to create new location 
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Guid of object have been created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateKitchenReq req)
        {
            var result = await _kitchenService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpDelete("{kitchenId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid kitchenId)
        {
            var result = await _kitchenService.Delete(kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut("{kitchenId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid kitchenId, UpdateKitchenReq req)
        {
            var result = await _kitchenService.Update(kitchenId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all Location with paging
        /// </summary>
        /// <param name="req"></param>
        /// <returns>
        /// Paging List of Location response
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<KitchenRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters req)
        {
            var result = await _kitchenService.GetAll(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get Kitchen by Id
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("{areaId}")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid areaId)
        {
            var result = await _kitchenService.GetById(areaId);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
