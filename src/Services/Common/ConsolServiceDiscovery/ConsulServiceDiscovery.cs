using Consul;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsolServiceDiscovery
{
  public  class ConsulServiceDiscovery : IConsulServiceDiscovery
    {
        private IConsulClient _consulclient;

        public ConsulServiceDiscovery(IConsulClient consulclient)
        {
            _consulclient = consulclient;
        }

      

        public async Task<Uri> GetRequestUriAsync(string serviceName, string routepath)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulclient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices, serviceName);

            if (service == null)
            {
                throw new ConsulServiceNotFoundException($"Consul service: '{serviceName}' was not found.",
                    serviceName);
            }

            var uriBuilder = new UriBuilder            {
                Host = service.Address,
                Port = service.Port
            };
            uriBuilder.Path = routepath;
           
                return uriBuilder.Uri;
        }

        private AgentService GetRandomInstance(IList<AgentService> services, string serviceName)
        {
            Random _random = new Random();

            AgentService servToUse = null;

            servToUse = services[_random.Next(0, services.Count)];

            return servToUse;
        }
    }
}
