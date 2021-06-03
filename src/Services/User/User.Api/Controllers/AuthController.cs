using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Infrastructure;
using  User.Domain;
using ResponseHandling;
using User.ViewModel.model;
using User.ViewModel;
using User.Application.Service.Abstract;

namespace User.Api.Controllers
{
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]


    public class AuthController : ControllerBase
    {
        private readonly IMongoContext _mongoContext;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IMongoContext mongoContext, IAuthService authService,IHttpContextAccessor httpContextAccessor)
        {
            _mongoContext = mongoContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Sign In method provides token which has to be used for authentication for the remaining calls
        /// </summary>
        /// <param name="UserSignInModel"></param>
        /// <returns></returns>
       [HttpPost]
       [Route("SignIn")]
        public async Task<ActionResult<ApiResponse<SignInResponseModel>>> SignIn(UserSignInModel UserSignInModel)
        {
            ApiResponse<SignInResponseModel> apiResponse = new ApiResponse<SignInResponseModel>();
            apiResponse.Response = await _authService.SignIn(UserSignInModel);
            apiResponse.InfoMsg = "Sign In successfull.";
            return Ok(apiResponse);
        }

        /// <summary>
        /// Gets Auth setting for the token validation
        /// </summary>
        /// <param name="AuthSettingId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetAuthSettings")]
        public async Task<ActionResult<AuthTokenSetting>> GetAuthSettings(string AuthSettingId)
        {
            var response = await _authService.GetAuthSettingsAsync(AuthSettingId);
            return Ok(response);
        }


        /// <summary>
        /// Inserts Auth setting this should not be exposed outside.
        /// </summary>
        /// <param name="AuthTokenSetting"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertAuthSettings")]
        public async Task<ActionResult<AuthTokenSetting>> InsertAuthSettings(AuthTokenSetting AuthTokenSetting)
        {
            var response = await _authService.InsertAuthSettingsAsync(AuthTokenSetting);
            return Ok(response);
        }

        /// <summary>
        /// Inserts ForgetPassword setting this should not be exposed outside.
        /// </summary>
        /// <param name="ForgetPasswordSettings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertForgetPasswordSettings")]
        public async Task<ActionResult> InsertForgetPasswordSettings(ForgetPasswordSettings ForgetPasswordSettings)
        {
             await _authService.InsertForgetPasswordSettings(ForgetPasswordSettings);
            return Ok();
        }

        /// <summary>
        /// Sends link to email to reset password
        /// </summary>
        /// <param name="forgetPassword"></param>
        /// <returns></returns>
        [HttpPost]
       [Route("ForgetPassword")]
        public async Task<ActionResult<ApiResponse<string>>> ForgetPassword(ForgetPassword forgetPassword)
        {
            ApiResponse<string> apiResponse = new ApiResponse<string>();
            await _authService.ForgetPassword(forgetPassword);
            apiResponse.InfoMsg = "Please check your mail for resetting password.";
            return apiResponse;
        }



        /// <summary>
        /// Reset forget password through parameter sent to mail id link.
        /// </summary>
        /// <param name="resetForgetPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetForgetPassword")]
        public async Task<ActionResult<ApiResponse<SignInResponseModel>>> ResetForgetPassword(ResetForgetPassword resetForgetPassword)
        {
            ApiResponse<SignInResponseModel> apiResponse = new ApiResponse<SignInResponseModel>();
            apiResponse.Response= await _authService.ResetForgetPassword(resetForgetPassword);
            apiResponse.InfoMsg = "Password reset successfull.";
            return Ok(apiResponse);
        }
       
        /// <summary>
        /// Reset password after the user logged in
        /// </summary>
        /// <param name="ResetPasswordViewModel"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword(ResetPasswordViewModel ResetPasswordViewModel)
        {
            ApiResponse<SignInResponseModel> apiResponse = new ApiResponse<SignInResponseModel>();
             await _authService.ResetPassword(ResetPasswordViewModel);
            apiResponse.InfoMsg = "Password reset successfull.";
            return Ok(apiResponse);
        }

    }
}
