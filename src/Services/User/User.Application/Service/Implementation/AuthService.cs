using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ResponseHandling;
using ServiceHeader;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Application.IRepository;
using User.Application.Security;
using User.Application.Service.Abstract;
using User.Domain;
using User.ViewModel.model;

namespace User.Application.Service.Implementation
{
   public class AuthService : IAuthService
    {
        private readonly IAuthTokenSettingRepository _authTokenSettingRepository;
        private readonly ISignInRepository _signInRepository;
        private readonly IUserRepository _userRepository;
        public readonly IForgetPasswordSettingsRepository _forgetPasswordSettingsRepository;
        private readonly IConfiguration _configuration;
        private readonly RequestInfo _requestInfo;
        public AuthService(IAuthTokenSettingRepository authTokenSettingRepository,
            ISignInRepository signInRepository, IUserRepository userRepository,
            IForgetPasswordSettingsRepository forgetPasswordSettingsRepository, 
            IConfiguration configuration,RequestInfo requestInfo)
        {
            _authTokenSettingRepository = authTokenSettingRepository;
            _signInRepository = signInRepository;
            _userRepository = userRepository;
            _forgetPasswordSettingsRepository = forgetPasswordSettingsRepository;
            _configuration = configuration;
            _requestInfo = requestInfo;
        }

     
        public async Task<SignInResponseModel> ResetForgetPassword(ResetForgetPassword resetForgetPassword)
        {
            ForgetPasswordSettings forgetPasswordSettings= await  _forgetPasswordSettingsRepository.GetSettingsByIdAsync(resetForgetPassword.Key);
            if(forgetPasswordSettings == null)throw new BusinessException("Link is invalid");
            
            string JsonString = NavalCrypto.DESDecrypt( resetForgetPassword.Token, forgetPasswordSettings.EncryptKey);
            ForgetPasswordSerializer forgetPasswordSerializer = JsonSerializer.Deserialize<ForgetPasswordSerializer>(JsonString);
            if (forgetPasswordSerializer == null) throw new BusinessException("Link is invalid");

            UserDto userDto= await _userRepository.GetUserByIdAsync(forgetPasswordSerializer.UserId);
            if (userDto == null) throw new BusinessException("Link is invalid");

            ResetPassword resetPassword = userDto.UserCredential.ResetPasswordKeys.FirstOrDefault(x => x.ResetPasswordKey == forgetPasswordSerializer.Key);
            if (resetPassword == null) throw new BusinessException("Link has been used up or invalid");

            if (resetPassword.ExpiresAt < DateTime.UtcNow) throw new BusinessException("Link has expired.");

            userDto.UserCredential.SaltedPassword = NavalCrypto.HashText(resetForgetPassword.Password, userDto.UserCredential.Salt);
            await _userRepository.ResetForgetPassword(userDto.Id, resetPassword.ResetPasswordKey, userDto.UserCredential.SaltedPassword);
            string Token = await GenerateJwtAndInsertSignInHistory(userDto.Id, userDto.Roles);
            return await FillUpSignInResponseModel(userDto, Token);


        }
        public async Task<SignInResponseModel> FillUpSignInResponseModel(UserDto userDto, string Token)
        {
            SignInResponseModel signInResponseModel = new SignInResponseModel();
            signInResponseModel.AccessToken = Token;
            signInResponseModel.EmailId = userDto.UserProfile.EmailId;
            signInResponseModel.PreferredName = userDto.UserProfile.PreferredName;
            return await Task.FromResult(signInResponseModel);
        }


        public async Task ForgetPassword(ForgetPassword ForgetPassword)
        {
         UserDto userDto =  await _userRepository.GetUserBymailIdAsync(ForgetPassword.EmailId);
            if (userDto == null)
                throw new BusinessException("Provided email is not registered with us.");
            ResetPassword resetPassword = new ResetPassword();

          ForgetPasswordSettings forgetPasswordSettings= await  _forgetPasswordSettingsRepository.GetSettingAsync();
          resetPassword.ResetPasswordKey = Guid.NewGuid().ToString();
          resetPassword.ExpiresAt = DateTime.UtcNow.AddMinutes(forgetPasswordSettings.ExpireAfterMinutes);
          var obj = new ForgetPasswordSerializer  { Key= resetPassword.ResetPasswordKey,UserId= userDto.Id };
          string ResetToken =  NavalCrypto.DESEncrypt(JsonSerializer.Serialize(obj), forgetPasswordSettings.EncryptKey);
          var param = new Dictionary<string, string> { { "Token", ResetToken },{ "Key", forgetPasswordSettings.Id } };
          var VerificationUrl = new Uri(QueryHelpers.AddQueryString(_configuration.GetValue<string>("ForgetPassword:Url"), param)).ToString();
          resetPassword.UrlForDevelopment = VerificationUrl;
          await _userRepository.AddResetPassword(resetPassword, userDto.Id);
          SendForgetPasswordEmailToQueue(userDto.UserProfile.EmailId, VerificationUrl, userDto.UserProfile.PreferredName, resetPassword.ExpiresAt);

        }

        public async void SendForgetPasswordEmailToQueue(string emailId,string VerificationUrl,string PreferredName,DateTime? ExpiesAt)
        {

        }
        public async Task<AuthTokenSetting> GetAuthSettingsAsync(string authSettingId)
        {
            return await _authTokenSettingRepository.GetSettingsByIdAsync(authSettingId);
        }


        public async Task ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            UserDto userDto = await _userRepository.GetUserByIdAsync(_requestInfo.UserId);
            if (userDto == null)
                throw new CriticalBusinessException("Unable to process request.Our team has been notified.");
            string providedpassword = NavalCrypto.HashText( resetPasswordViewModel.OldPassword, userDto.UserCredential.Salt);
            if (providedpassword != userDto.UserCredential.SaltedPassword)
                throw new BusinessException("Password does not match.");
            userDto.UserCredential.SaltedPassword = NavalCrypto.HashText(resetPasswordViewModel.NewPassword, userDto.UserCredential.Salt);
            UpdateResult updateResult= await _userRepository.ResetPassword(userDto.Id, userDto.UserCredential.SaltedPassword);
            if(updateResult.ModifiedCount !=1 && !updateResult.IsAcknowledged)
            {
                throw new CriticalBusinessException("Unable to process request.Our team has been notified.","Mofified count is not available");
            }
            return;
        }
        public async Task<SignInResponseModel> SignIn(UserSignInModel userSignInModel)
        {
           UserDto userDto = await _userRepository.GetUserBymailIdAsync(userSignInModel.EmailId);
            if (userDto == null)
                throw new BusinessException("Provided email does not exists.");
           string providedpassword=  NavalCrypto.HashText(userDto.UserCredential.Salt, userSignInModel.Password);
            if (providedpassword != userDto.UserCredential.SaltedPassword)
                throw new UnauthorizedAccessException("Password does not match.");
            string Token = await GenerateJwtAndInsertSignInHistory(userDto.Id, userDto.Roles);
            return await FillUpSignInResponseModel(userDto, Token);
        }


        public async Task<AuthTokenSetting> InsertAuthSettingsAsync(AuthTokenSetting AuthTokenSetting)
        {
            await _authTokenSettingRepository.InsertSettingsAsync(AuthTokenSetting);
            return AuthTokenSetting;
        }

        public async Task InsertForgetPasswordSettings(ForgetPasswordSettings forgetPasswordSettings)
        {
            await _forgetPasswordSettingsRepository.InsertSettingsAsync(forgetPasswordSettings);
        }
        
        public async Task<string> GenerateJwtAndInsertSignInHistory(string UserId, List<string> Roles, DateTime? ExpirationTime = null)
        {
            string jti = Guid.NewGuid().ToString();
            var authsettings = await _authTokenSettingRepository.GetSettingAsync(DateTime.UtcNow);
            if (ExpirationTime == null || ExpirationTime.Value == DateTime.MinValue)
            {
                ExpirationTime = DateTime.UtcNow.AddMonths(authsettings.TokenValidityMonths);
            }
            string jwt = await GenerateJwt(UserId, jti, ExpirationTime, Roles, authsettings);
            await _signInRepository.InsertSignIn(authsettings.Id,UserId,jti,DateTime.UtcNow, ExpirationTime.Value);
            return jwt;
        }

        

        /// <summary>
        /// generates JWT 
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="jti">User SignIn Id</param>
        /// <param name="ExpirationTime"></param>
        /// <returns></returns>
        private async Task<string> GenerateJwt(string UserId,string jti, DateTime? ExpirationTime,List<string>Roles, AuthTokenSetting authsettings)
        {
            

            
            byte[] Symmetrickey = Encoding.ASCII.GetBytes(authsettings.SecretKey);
            SecurityKey SecurityKey = new SymmetricSecurityKey(Symmetrickey);
            var securityToken = new JwtSecurityToken
             (
            claims: GetClaims(UserId,jti, Roles,authsettings.Id),
            expires: ExpirationTime,
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256)
            );
            var handler = new JwtSecurityTokenHandler();
            return await Task.FromResult(handler.WriteToken(securityToken));
        }


       /// <summary>
       /// Returns claims for jwt generation
       /// </summary>
       /// <param name="UserId"></param>
       /// <param name="jti"></param>
       /// <param name="userRoles"></param>
       /// <returns></returns>
        private IEnumerable<Claim> GetClaims(string UserId, string jti, List<string> userRoles,string AuthSettingId)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim("UserId",UserId),
            new Claim("AuthId",AuthSettingId),
            new Claim(type:JwtRegisteredClaimNames.Iat,value:Convert.ToString((Int32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)),
        };
            if (userRoles != null)
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(type: "Role", value: role));
                }
            }
            return claims;
        }
    }
}
