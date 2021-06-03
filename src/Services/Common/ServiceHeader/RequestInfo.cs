using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ServiceHeader
{
   public class RequestInfo
    {
        public string UserId { get; set; }
        public string RequestId { get; set; }

        public string RoutePath { get; set; }
        public string Jti { get; set; }
        public string Role { get; internal set; }
    }

    public class RequestInfoMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, RequestInfo requestInfo)
        {
            if(requestInfo.UserId == null  && context.Request.Headers.ContainsKey("UserSerialId"))
            {
                requestInfo.UserId = context.Request.Headers.FirstOrDefault(x => x.Key == "UserSerialId").Value;
                requestInfo.RoutePath = context.Request.Path;
                requestInfo.Jti = context.Request.Headers.FirstOrDefault(x => x.Key == "jti").Value;
                requestInfo.Role= context.Request.Headers.FirstOrDefault(x => x.Key == "Role").Value;
            }

            await _next(context);
        }
    }
    public static class RegisterRequestInfoMiddleware
    {
        public static void AddRequestInfo(this IServiceCollection services)
        {
            services.AddScoped<RequestInfo>();
        }
        public static IApplicationBuilder UseRequestInfo(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<RequestInfoMiddleware>();
        }
    }
}
