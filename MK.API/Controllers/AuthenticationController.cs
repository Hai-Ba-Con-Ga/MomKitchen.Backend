using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Application.Service;
using MK.Domain.Constant;
using MK.Domain.Dto.Request;
using MK.Domain.Dto.Response;
using MK.Domain.Entity;
using System.Reflection.Metadata;

namespace MK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            this._authenticationService = authenticationService;
            this._userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string token)
        {
            var user = await _authenticationService.GetUserByFirebaseTokenAsync(token);
            LoginResponse loginResponse = new();
            if (user is null)
            {
                loginResponse.IsFirstTime = true;
            }
            else
            {
                loginResponse.IsFirstTime = false;
                loginResponse.Token = _authenticationService.GenerateToken(user);
                SetCookie(AppConstant.COOKIE_NAME, loginResponse.Token);
            }
            return Ok(loginResponse);
        }

        [HttpPost("log-out")]
        public IActionResult Logout()
        {
            RemoveCookie(AppConstant.COOKIE_NAME);
            return Ok();
        }

        [HttpPost("first-time-login")]

        public async Task<IActionResult> FirstTimeLogin([FromBody] FirstTimeRequest loginRequest)
        {
            var user = await _authenticationService.GetUserByFirebaseTokenAsync(loginRequest.FirebaseToken);
            if (user is not null)
            {
                return BadRequest("User already exited!");
            }
            var firebaseToken = await _authenticationService.GetFirebaseTokenAsync(loginRequest.FirebaseToken);
            var email = firebaseToken.Claims.GetValueOrDefault("email");
            if (email is null)
            {
                throw new Exception($"This account cannot use to log-in, please try another account!");
            }
            User userEntity = new User();
            userEntity.Email = email.ToString();
            userEntity = await _userService.SignUpUserAsync(userEntity);

            LoginResponse loginResponse = new();
            loginResponse.Token = _authenticationService.GenerateToken(userEntity);
            loginResponse.IsFirstTime = true;
            SetCookie(AppConstant.COOKIE_NAME, loginResponse.Token);
            return Ok(loginResponse);
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
