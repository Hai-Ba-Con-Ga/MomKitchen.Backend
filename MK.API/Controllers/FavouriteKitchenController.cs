using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FavouriteKitchenController : ControllerBase
    {
        private readonly FavouriteKitchenService _favouriteKitchenService;

        public FavouriteKitchenController()
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Guid customerId, Guid kitchenId)
        {
            var result = await _favouriteKitchenService.Create(customerId, kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] Guid customerId, Guid kitchenId)
        {
            var result = await _favouriteKitchenService.Delete(customerId, kitchenId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<FavouriteKitchenRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] PaginationParameters pagingParam)
        {
            var result = await _favouriteKitchenService.Get(pagingParam);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
