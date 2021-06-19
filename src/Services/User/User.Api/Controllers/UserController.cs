using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponseHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Application.Service.Abstract;
using User.Infrastructure;
using User.ViewModel;
using User.ViewModel.model;

namespace User.Api.Controllers
{
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {

        private readonly IMongoContext _mongoContext;
        private readonly IUserService _userService;
        public UserController(IMongoContext mongoContext, IUserService userService)
        {
            (_mongoContext,_userService) = (mongoContext,userService);
        }

        /// <summary>
        /// User Sign Up Method.
        /// </summary>
        /// <param name="userSignUpModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult<ApiResponse<SignInResponseModel>>> SignUp(UserSignUpModel userSignUpModel)
        {
            ApiResponse<SignInResponseModel> ApiResponse = new ApiResponse<SignInResponseModel>();
            ApiResponse.Response= await _userService.RegisterUser(userSignUpModel);
            ApiResponse.InfoMsg = "Sign Up successfull.";
            return Ok(ApiResponse);   
        }




        /// <summary>
        /// User Sign Up Method.
        /// </summary>
        /// <param name="userSignUpModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignUpAsTutor")]
        public async Task<ActionResult<ApiResponse<SignInResponseModel>>> SignUpAsTutor(UserSignUpModel userSignUpModel)
        {
            ApiResponse<SignInResponseModel> ApiResponse = new ApiResponse<SignInResponseModel>();
            ApiResponse.Response = await _userService.RegisterUser(userSignUpModel);
            ApiResponse.InfoMsg = "Sign Up successfull.";
            return Ok(ApiResponse);
        }

      
        /// <summary>
        /// checks if the user is already a tutor. If already a tutor then return a jwt with titor role.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckBecomeTutor")]
        public async Task<ActionResult<string>> CheckBecomeTutor()
        {
            ApiResponse<string> ApiResponse = new ApiResponse<string>();
            ApiResponse.Response = await _userService.CheckBecomeTutor();
            return Ok(ApiResponse);
        }

        




    }
}
