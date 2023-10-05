using Newtonsoft.Json;

namespace MK.API.Configuration
{
    public static class ConfigureApiBehavior
    {
        public static IMvcBuilder AddConfigApiBehaviorOptions(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            { // handle validation response 
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                    errors = errors.Distinct();

                    var response = new ResponseObject<string>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = JsonConvert.SerializeObject(errors)
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return builder;
        }

    }
}
