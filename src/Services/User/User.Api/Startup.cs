using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ConsolServiceDiscovery;
using User.Infrastructure;
using ResponseHandling;
using System.Net.Mime;
using User.Application.Service;
using ServiceHeader;

namespace User.Api
{

    /// <summary>
    /// Entry point of api
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Interface to access appsettings
        /// </summary>
        public IConfiguration Configuration { get; }

        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader(); });
            });
            services.AddRequestInfo();
            services.AddControllers();
            services.AddLogicsServices();
            services.AddAutoMapper(typeof(User.ViewModel.Mapper));
            services.ConfigureSwagger();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                    return result;
                };
            });
            services.AddHttpContextAccessor();
           

            services.AddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(Configuration.GetConnectionString("DefaultConnectionString"));
            });

            services.AddScoped(c =>
            c.GetService<MongoClient>().StartSession());
            services.AddInfrastructureServices();
            services.AddConsul(Configuration.GetConsolServiceConfig());
            ConfigureConsul(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {

            app.UseRequestInfo();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }
            
            app.UseResponseExceptionHandler();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

      

        private void ConfigureConsul(IServiceCollection services)
        {
            var serviceConfig = Configuration.GetConsolServiceConfig();

            services.RegisterServicesInConsul(serviceConfig);
        }

      
    }
}
