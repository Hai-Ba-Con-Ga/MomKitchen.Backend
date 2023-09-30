using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MK.Application.Service;
using MK.Domain.Constant;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using MK.Domain.Entity;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace MK.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        /// <summary>
        /// Funtion to register new user or login if user already exists
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authenticationService.GetUserByFirebaseTokenAsync(loginRequest);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                SetCookie(AppConstant.COOKIE_NAME, result.Data.Token);
            }
            
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to logout
        /// </summary>
        /// 

        [HttpDelete]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest)
        {
            RemoveCookie(AppConstant.COOKIE_NAME);
            string rawUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (rawUserId.IsNullOrEmpty())
            {
                return StatusCode((int)HttpStatusCode.OK, new { message = "Invalid token" });
            }
            else
            {
                Guid userId = Guid.Parse(rawUserId);
                var result = await _authenticationService.Logout(userId, logoutRequest.FcmToken);
                return StatusCode((int)result.StatusCode, result);
            }
        }

        private void SetCookie(string key, string value)
        {
            CookieOptions cookieOptions = new();
            cookieOptions.HttpOnly = true;
            cookieOptions.Expires = DateTime.Now.AddDays(2);
            HttpContext.Response.Cookies.Append(key, value);
        }

        private void RemoveCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

    }
}
