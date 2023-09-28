using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authenticationService.GetUserByFirebaseTokenAsync(loginRequest);
            if (result == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            SetCookie(AppConstant.COOKIE_NAME, result.Token);
            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout([FromBody] LogoutRequest logoutRequest)
        {
            RemoveCookie(AppConstant.COOKIE_NAME);
            string rawUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawUserId == null)
            {
                return BadRequest(new { message = "User is not logged in" });
            }
            else
            {
                Guid userId = Guid.Parse(rawUserId);
                _authenticationService.Logout(userId, logoutRequest.FcmToken);
            }

            return Ok();
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
