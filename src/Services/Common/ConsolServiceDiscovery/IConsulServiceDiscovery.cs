using System;
using System.Threading.Tasks;

namespace ConsolServiceDiscovery
{
    public interface IConsulServiceDiscovery
    {
        Task<Uri> GetRequestUriAsync(string serviceName, string RoutePath);
    }
}