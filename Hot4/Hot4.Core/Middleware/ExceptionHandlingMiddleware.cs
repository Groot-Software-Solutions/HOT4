
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Hot4.Core.Middleware
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Detail = exception.StackTrace
            };

            // return await context.Response.WriteAsJsonAsync(errorResponse);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
