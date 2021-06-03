using System;

namespace ConsolServiceDiscovery
{
    public class ServiceConfig
    {

        /// <summary>
        /// url of consul
        /// </summary>
        public Uri ServiceDiscoveryAddress { get; set; }


        /// <summary>
        /// url of service to register
        /// </summary>
        public Uri ServiceAddress { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
    }
}
