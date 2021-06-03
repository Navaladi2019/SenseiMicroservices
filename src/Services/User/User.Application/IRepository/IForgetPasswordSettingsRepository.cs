using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application.IRepository
{
  public  interface IForgetPasswordSettingsRepository
    {
        public Task<ForgetPasswordSettings> GetSettingAsync(DateTime? dateTime = null);

        Task<ForgetPasswordSettings> GetSettingsByIdAsync(string SettingId);

        Task InsertSettingsAsync(ForgetPasswordSettings ForgetPasswordSettings);
    }
}
