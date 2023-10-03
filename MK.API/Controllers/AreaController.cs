using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MK.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
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

        ///// <summary>
        ///// Function to soft delete location
        ///// </summary>
        ///// <param name="locationId"></param>
        ///// <returns></returns>
        //[HttpDelete("{locationId}")]
        //[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Delete(Guid locationId)
        //{
        //    var result = await _locationService.Delete(locationId);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        ///// <summary>
        ///// Function to update location
        ///// </summary>
        ///// <param name="locationId"></param>
        ///// <returns></returns>
        //[HttpPut("{locationId}")]
        //[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Delete(Guid locationId, UpdateLocationReq req)
        //{
        //    var result = await _locationService.Update(locationId, req);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        ///// <summary>
        ///// Function to get all location with paging and filter
        ///// </summary>
        ///// <param name="pageNumer">Not Require</param>
        ///// <param name="pageSize">Not Require</param>
        ///// <returns>Paging list of location</returns>
        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<LocationRes>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAll(int? pageNumer = null, int? pageSize = null)
        //{
        //    var result = await _locationService.GetAll(new PaginationParameters
        //    {
        //        PageNumber = pageNumer ?? 1,
        //        PageSize = pageSize ?? 10
        //    });
        //    return StatusCode((int)result.StatusCode, result);
        //}

        ///// <summary>
        ///// Get location by id
        ///// </summary>
        ///// <param name="locationId"></param>
        ///// <returns></returns>
        //[HttpGet("{locationId}")]
        //[ProducesResponseType(typeof(LocationRes), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetById(Guid locationId)
        //{
        //    var result = await _locationService.GetById(locationId);
        //    return StatusCode((int)result.StatusCode, result);
        //}
    }
}
