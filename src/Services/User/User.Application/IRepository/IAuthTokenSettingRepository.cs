using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application.IRepository
{
    public   interface IAuthTokenSettingRepository
    {

        public Task<AuthTokenSetting> GetSettingAsync(DateTime? dateTime = null);

        Task<AuthTokenSetting> GetSettingsByIdAsync(string authSettingId);

        Task InsertSettingsAsync(AuthTokenSetting authTokenSetting);
    }
}
