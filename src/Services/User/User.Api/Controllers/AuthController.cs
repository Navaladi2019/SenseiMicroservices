using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Controllers
{
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]


    public class AuthController : ControllerBase
    {
        [HttpGet]     
     
        public int ok()
        {
            return 8;
        }

        [HttpGet]
        [Route("2")]
        [ApiVersion("2.0")]
        public int oks()
        {
            return 8;
        }
    }
}
