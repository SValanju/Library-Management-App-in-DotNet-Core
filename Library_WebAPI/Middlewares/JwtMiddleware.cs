using Library_WebAPI.DTOs;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Helpers.Utils;
using Library_WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Library_WebAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string? token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ")[1];

            if (token == null)
            {
                // check if incoming request is from a enabled auauthorized route
                if (IsEnabledUnauthorizedRoute(httpContext))
                {
                    return _next(httpContext);
                }

                BaseResponse response = IGlobalService.GetResponse(System.Net.HttpStatusCode.Unauthorized, new MessageDTO("Unauthorized"));
                httpContext.Response.StatusCode = response.status_code;
                httpContext.Response.ContentType = "application/json";
                return httpContext.Response.WriteAsJsonAsync(response);
            }
            else
            {
                if (JwtUtils.ValidateJwtToken(token))
                {
                    return _next(httpContext);
                }

                BaseResponse response = IGlobalService.GetResponse(System.Net.HttpStatusCode.Unauthorized, new MessageDTO("Unauthorized"));
                httpContext.Response.StatusCode = response.status_code;
                httpContext.Response.ContentType = "application/json";
                return httpContext.Response.WriteAsJsonAsync(response);
            }
        }

        /// <summary>
        ///     This method is used to check incoming request is from a enabled unauthorized request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool IsEnabledUnauthorizedRoute(HttpContext httpContext)
        {
            List<string> enabledRoutes = new List<string>
            {
                "/api/User/adduser",
                "/api/Auth/login"
            };

            bool isEnableUnauthorizedRoute = false;

            if(httpContext.Request.Path.Value is not null)
            {
                isEnableUnauthorizedRoute = enabledRoutes.Contains(httpContext.Request.Path.Value.ToString());
            }

            return isEnableUnauthorizedRoute;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
