using ConsolServiceDiscovery;
using Consul;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace InterServiceCommunication.User
{

   public class UserInterService : IUserInterService
    {
        private readonly HttpClient _client;
        private IConsulServiceDiscovery _consulclient;
        private readonly IConfiguration _configuration;
        private readonly string Servicename;
        public  UserInterService(HttpClient client, IConsulServiceDiscovery consulclient, IConfiguration configuration)
        {
            _client = client;
            _consulclient = consulclient;
            _configuration = configuration;
            Servicename = _configuration.GetValue<string>("InterServiceName:sensei_user_service");
        }

        public async  Task<AuthTokenSetting> GethashKeyForAuthentication(string AuthSettingId)
        {
            
            var baseaddress = await _consulclient.GetRequestUriAsync(Servicename, "/v1/Auth/GetAuthSettings");
            var param = new Dictionary<string, string>();
            param.Add("AuthSettingId", AuthSettingId);
            var uri= new Uri(QueryHelpers.AddQueryString(baseaddress.ToString(), param));
           
            var response = await _client.GetAsync(uri);
            return await response.ReadContentAs<AuthTokenSetting>();
        }

    }
}
