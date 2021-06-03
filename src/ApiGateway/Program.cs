using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                     {
                         //config.AddOcelotWithSwaggerSupport(o=>{
                         //    o.Folder = "Ocelot";
                         //   });

                         //config.AddOcelot( "./Ocelot/", hostingContext.HostingEnvironment).AddEnvironmentVariables();
                         // config.AddOcelot(hostingContext.HostingEnvironment).AddEnvironmentVariables();
                         config.AddJsonFile("ocelot.json").AddEnvironmentVariables();
                     });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
