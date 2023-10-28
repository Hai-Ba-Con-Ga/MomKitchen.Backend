using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Response.Tray;
using MK.Domain.Dto;
using System.ComponentModel.DataAnnotations;
using MK.Domain.Dto.Request.Tray;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TrayController : ControllerBase
    {
        private readonly ITrayService _trayService;
        public TrayController(ITrayService trayService)
        {
            _trayService = trayService;
        }


        /// <summary>
        /// Function to create new tray
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateTrayReq req)
        {
            var result = await _trayService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update tray
        /// </summary>
        /// <param name="trayId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] Guid trayId, [FromBody] UpdateTrayReq req)
        {
            var result = await _trayService.Update(trayId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to delete tray
        /// </summary>
        /// <param name="trayId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] Guid trayId)
        {
            var result = await _trayService.Delete(trayId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all tray
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagingResponse<TrayRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters pagingParam, [FromQuery] string[] fields)
        {
            var result = await _trayService.GetAll(pagingParam, fields);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get tray by id
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <returns></returns>
        [HttpGet("{trayId}")]
        [ProducesResponseType(typeof(ResponseObject<TrayDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid trayId)
        {
            var result = await _trayService.GetTrayById(trayId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
