using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Kitchen;
using MK.Service.Common;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;
        private readonly IDishService _dishService;
        private readonly ITrayService _trayService;
        private readonly IMealService _mealService;

        public KitchenController(
            IKitchenService kitchenService,
            IDishService dishService,
            ITrayService trayService,
            IMealService mealService)
        {
            _kitchenService = kitchenService;
            _dishService = dishService;
            _trayService = trayService;
            _mealService = mealService;
        }

        /// <summary>
        /// Function to create new Kitchen 
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

        /// <summary>
        /// Function to delete kitchen
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <returns></returns>
        [HttpDelete("{kitchenId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid kitchenId)
        {
            var result = await _kitchenService.Delete(kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update kitchen
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{kitchenId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid kitchenId, UpdateKitchenReq req)
        {
            var result = await _kitchenService.Update(kitchenId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all Kitchen with paging
        /// </summary>
        /// <param name="req"></param>
        /// <returns>
        /// Paging List of Location response
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagingResponse<KitchenRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string[]? fields, [FromQuery] PagingParameters req)
        {
            if (fields != null && !fields.IsMatchFieldPattern())
            {
                return BadRequest(new ResponseObject<KitchenRes>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Fields are not valid, they are not match with pattern [fieldName:action]"
                });
            }

            var result = await _kitchenService.GetAll(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get Kitchen by Id
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("{kitchenId}")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([Required] Guid kitchenId)
        {
            var result = await _kitchenService.GetById(kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all dishes of a kitchen
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("{kitchenId}/dishes")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDishByKitchenId([Required] Guid kitchenId, [FromQuery] PagingParameters req)
        {
            var result = await _dishService.GetDishesByKitchenId(kitchenId, req ?? new PagingParameters());
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all trays of a kitchen
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("{kitchenId}/trays")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTrayByKitchenId([Required] Guid kitchenId, [FromQuery] PagingParameters req)
        {
            var result = await _trayService.GetTraysByKitchenId(kitchenId, req ?? new PagingParameters());
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all meals of a kitchen
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("{kitchenId}/meals")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMealsByKitchenId([Required] Guid kitchenId, [FromQuery] PagingParameters req)
        {
            var result = await _mealService.GetMealsByKitchenId(kitchenId, req ?? new PagingParameters());
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all kitchen by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(KitchenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByUserId([Required] Guid userId, [FromQuery] PagingParameters req)
        {
            var result = await _kitchenService.GetKitchensByUserId(userId, req ?? new PagingParameters());
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
