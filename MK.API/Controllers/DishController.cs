using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto;
using MK.Domain.Dto.Response.Dish;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Function to create new dish
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateDishReq req)
        {
            var result = await _dishService.CreateDish(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update dish
        /// </summary>
        /// <param name="dishId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] Guid dishId, [FromBody] UpdateDishReq req)
        {
            var result = await _dishService.UpdateDish(dishId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to delete dish
        /// </summary>
        /// <param name="dishId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] Guid dishId)
        {
            var result = await _dishService.DeleteDish(dishId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all dish
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagingResponse<DishRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters pagingParam, [FromQuery] string[] fields)
        {
            var result = await _dishService.GetAllDish(pagingParam, fields);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get dish by id
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <returns></returns>
        [HttpGet("{kitchenId}")]
        [ProducesResponseType(typeof(ResponseObject<DishDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid kitchenId)
        {
            var result = await _dishService.GetDishById(kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
