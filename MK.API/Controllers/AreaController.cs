using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }


        /// <summary>
        /// Function to create new location 
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Guid of object have been created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateAreaReq req)
        {
            var result = await _areaService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to soft delete location
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpDelete("{areaId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid areaId)
        {
            var result = await _areaService.Delete(areaId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update area 
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{areaId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid areaId, UpdateAreaReq req)
        {
            var result = await _areaService.Update(areaId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all location with paging and filter
        /// </summary>
        /// <returns>Paging list of location</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAreaRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters queryParam)
        {
            var result = await _areaService.GetAll(queryParam);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get area by id
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("{areaId}")]
        [ProducesResponseType(typeof(GetAreaRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid areaId)
        {
            var result = await _areaService.GetById(areaId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
