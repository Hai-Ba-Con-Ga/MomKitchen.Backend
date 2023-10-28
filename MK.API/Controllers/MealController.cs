using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Meal;
using MK.Domain.Dto.Response.Meal;
using static Google.Apis.Requests.BatchRequest;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        /// <summary>
        /// Function to get detail of meal by mealId
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns></returns>
        [HttpGet("{mealId}")]
        [ProducesResponseType(typeof(MealDetailRes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid mealId)
        {
            var response = await _mealService.GetMealById(mealId);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Function to get all meals with paging
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<MealRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters? pagingParam, [FromQuery] string[]? fields)
        {
            var response = await _mealService.GetAll(pagingParam, fields);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Function to create new meal
        /// </summary>
        /// <param name="createData"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseObject<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMeal(CreateMealReq createData)
        {
            var response = await _mealService.CreateMeal(createData);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Function to update meal
        /// </summary>
        /// <param name="mealId"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        [HttpPut("{mealId}")]
        [ProducesResponseType(typeof(ResponseObject<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMeal(Guid mealId, [FromBody] UpdateMealReq updateData)
        {
            var response = await _mealService.UpdateMeal(mealId, updateData);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Function to delete meal
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns></returns>
        [HttpDelete("{mealId}")]
        [ProducesResponseType(typeof(ResponseObject<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMeal(Guid mealId)
        {
            var response = await _mealService.DeleteMeal(mealId);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
