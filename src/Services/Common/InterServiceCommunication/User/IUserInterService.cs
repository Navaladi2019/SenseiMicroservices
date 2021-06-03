using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace InterServiceCommunication.User
{
   public interface IUserInterService
    {
        public Task<AuthTokenSetting> GethashKeyForAuthentication(string AuthSettingId);
    }
}
