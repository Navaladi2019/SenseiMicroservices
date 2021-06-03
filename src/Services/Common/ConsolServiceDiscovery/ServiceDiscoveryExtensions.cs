using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Consul;

namespace ConsolServiceDiscovery
{
 public  static  class ServiceDiscoveryExtensions
    {
        public static void AddConsul(this IServiceCollection services, ServiceConfig serviceConfig)
        {
            var consulClient = new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            }); ;
            services.AddSingleton<IConsulClient, ConsulClient>(p => consulClient);
            services.AddSingleton<IConsulServiceDiscovery, ConsulServiceDiscovery>();
        }


        public static void RegisterServicesInConsul(this IServiceCollection services, ServiceConfig serviceConfig)
        {
            if (serviceConfig == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig));
            }
            services.AddSingleton(serviceConfig);
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            
        }


    }
}
