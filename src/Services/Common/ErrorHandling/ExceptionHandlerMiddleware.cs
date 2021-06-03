using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ResponseHandling
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ApiResponse<string> Genericresponse = new ApiResponse<string>();
                Genericresponse.Response = "Request Failed.";
                string ExMsg = string.Empty;
                switch (error)
                {


                    case BusinessException e:
                        // custom application error
                        response.StatusCode = (int)e.StatusCode;
                        Genericresponse.SupportMessages = e.SupportingMessages;
                        ExMsg = e.Message;
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        ExMsg = e.Message;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        Genericresponse.SupportMessages.Add(error.Message);
                        ExMsg = "Unable to process with the request";
                        break;
                }



                Genericresponse.ErrMsg = ExMsg;
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var result = JsonSerializer.Serialize(Genericresponse, options);
                await response.WriteAsync(result);
            }
        }
    }
}

