using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConsolServiceDiscovery
{
  public static  class ServiceConfigExtensions
    {
        public static ServiceConfig GetConsolServiceConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serviceConfig = new ServiceConfig();


            if (!string.IsNullOrWhiteSpace(configuration.GetValue<string>("ServiceConfig:serviceDiscoveryAddress")))
            {
                serviceConfig.ServiceDiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:serviceDiscoveryAddress");
            }
            if (!string.IsNullOrWhiteSpace(configuration.GetValue<string>("ServiceConfig:serviceAddress")))
            {
                serviceConfig.ServiceAddress = configuration.GetValue<Uri>("ServiceConfig:serviceAddress");
            }
            if (!string.IsNullOrWhiteSpace(configuration.GetValue<string>("ServiceConfig:serviceName")))
            {
                serviceConfig.ServiceName = configuration.GetValue<string>("ServiceConfig:serviceName");
            }
            if (!string.IsNullOrWhiteSpace(configuration.GetValue<string>("ServiceConfig:serviceId")))
            {
                serviceConfig.ServiceId = configuration.GetValue<string>("ServiceConfig:serviceId");
            }

           

            return serviceConfig;
        }
    }
}
