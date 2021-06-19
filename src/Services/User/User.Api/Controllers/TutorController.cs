using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponseHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Application.Service.Abstract;
using User.Application.Service.Implementation;
using User.ViewModel.model;

namespace User.Api.Controllers
{
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TutorController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly ITutorService _tutorService;
        public TutorController(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }


        /// <summary>
        /// checks if the user is already a tutor. If already a tutor then return a jwt with titor role.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("BecomeTutor")]
        public async Task<ActionResult<string>> BecomeTutor(TutorBio TutorBio)
        {
            ApiResponse<string> ApiResponse = new ApiResponse<string>();
            ApiResponse.Response = await _tutorService.BecomeATutor(TutorBio);
            return Ok(ApiResponse);
        }
    }
}
