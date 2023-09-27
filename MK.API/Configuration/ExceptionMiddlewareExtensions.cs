using Microsoft.AspNetCore.Diagnostics;
using MK.Domain.Common;
using Newtonsoft.Json;
using System.Net;

namespace MK.API.Configuration
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, bool isDevelopmentEnvironment)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        ResponseObject<string> response;
                        if (!isDevelopmentEnvironment)
                        {
                            response = new ResponseObject<string>
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = "Have error, please try again later!"
                            };
                        }
                        else
                        {
                            response = new ResponseObject<string>
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = contextFeature.Error.Message
                            };
                        }

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }
                });
            });
        }
    }
}
