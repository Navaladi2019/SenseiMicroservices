using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Authentication
{
    public class UnauthorisedError : Error
    {

        public UnauthorisedError(string errorMessage, OcelotErrorCode ocelotErrorCode = OcelotErrorCode.UnauthorizedError,int httpstatuscode = 403):base(errorMessage, ocelotErrorCode, httpstatuscode)
        {

        }
    }
}
