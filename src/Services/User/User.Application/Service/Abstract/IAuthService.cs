using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.ViewModel.model;

namespace User.Application.Service.Abstract
{
  public  interface IAuthService
    {

       public Task<string> GenerateJwtAndInsertSignInHistory(string UserId,List<string>Roles,DateTime?ExpirationTime = null);
       public Task<SignInResponseModel> SignIn(UserSignInModel userSignInModel);
       public Task<AuthTokenSetting> GetAuthSettingsAsync(string authSettingId);

        public Task<SignInResponseModel> FillUpSignInResponseModel(UserDto userDto, string Token);

        public Task<AuthTokenSetting> InsertAuthSettingsAsync(AuthTokenSetting AuthTokenSetting);

        public Task ForgetPassword(ForgetPassword ForgetPassword);
        Task InsertForgetPasswordSettings(ForgetPasswordSettings forgetPasswordSettings);
        Task<SignInResponseModel> ResetForgetPassword(ResetForgetPassword resetForgetPassword);
        Task ResetPassword(ResetPasswordViewModel resetPasswordViewModel);
    }
}
