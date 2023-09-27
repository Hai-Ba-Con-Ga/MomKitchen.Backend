using Newtonsoft.Json;
using System.Net;

namespace MK.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILoggerManager _logger;
        public ExceptionMiddleware(RequestDelegate next)
        {

            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ResponseObject<string>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Internal Server Error from the custom middleware."
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
