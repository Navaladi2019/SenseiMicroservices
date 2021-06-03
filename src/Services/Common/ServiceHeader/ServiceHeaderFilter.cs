using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net.Http;

namespace ServiceHeader
{
    public class ServiceHeaderFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "UserSerialId",
                In = ParameterLocation.Header,
                Required = false,
                Example = new OpenApiString(Guid.NewGuid().ToString())
            });


            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Role",
                In = ParameterLocation.Header,
                Required = false,
                Example = new OpenApiString("Tutor,Student")
            });

        }
    }
}
