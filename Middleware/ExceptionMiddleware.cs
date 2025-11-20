using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cooktel_E_commrece.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(IHostEnvironment env, ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context) {

            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "Application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = _env.IsDevelopment() ?
                    new APIException(ex.Message, ex.StackTrace.ToString(), context.Response.StatusCode) :
                    new APIException(ex.Message,"internal server Error", context.Response.StatusCode);
               // var json =JsonSerializer.Serialize(response,new JsonSerializerOptions { PropertyNamingPolicy=JsonNamingPolicy.CamelCase});
                
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
