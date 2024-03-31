using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Library_WebAPI.Services;

namespace Library_WebAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception handled by middleware ...");

                BaseResponse response = IGlobalService.GetResponse(System.Net.HttpStatusCode.InternalServerError, new MessageDTO(ex.Message));
                httpContext.Response.StatusCode = response.status_code;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
