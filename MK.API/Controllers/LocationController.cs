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

        public LocationController()
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, typeof(IEnumerable<LocationRes>))]
        public Task<IActionResult> GetAll()
        {

        }
    }
}
