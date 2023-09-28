using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Response;

namespace MK.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Function to get all location with paging and filter
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(string[] fields, [FromQuery] PaginationParameters pParam)
        {
            var result = await _locationService.GetAll();
            return Ok(result);
        }
    }
}
