using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.User;

namespace MK.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Function to create new user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserReq userRequest)
        {
            var result = await _userService.Create(userRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserReq userRequest)
        {
            var result = await _userService.Update(id, userRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            return StatusCode((int)result.StatusCode, result);
        }
        //TODO: add paging get all filter


        /// <summary>
        /// Function to get all user with paging and filter
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(string searchKey, string? roleName = "Customer",[FromQuery] PagingParameters queryParam = null)
        {
            var result = await _userService.GetAll(roleName, searchKey, queryParam);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] string roleName)
        {
            var result = await _userService.UpdateRole(id, roleName);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
