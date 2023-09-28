﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Location;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
        /// <param name="pageNumer">Not Require</param>
        /// <param name="pageSize">Not Require</param>
        /// <returns>Paging list of location</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(int? pageNumer = null, int? pageSize = null)
        {
            var result = await _locationService.GetAll(new PaginationParameters
            {
                PageNumber = pageNumer ?? 1,
                PageSize = pageSize ?? 10
            });
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateLocationReq req)
        {
            var result = await _locationService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
