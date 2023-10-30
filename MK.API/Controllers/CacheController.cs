using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Application.Cache;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheManager _cacheManager;

        public CacheController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string key)
        {
            var (result, data) = await _cacheManager.GetAsync<string>(key);

            if (result)
                return Ok(data);

            return BadRequest(data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Set(string key, string value)
        {
            await _cacheManager.SetAsync(key, value);

            return Ok();
        }

    }
}
