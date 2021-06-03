using System;
using System.Collections.Generic;
using System.Net;

namespace ResponseHandling
{
    /// <summary>
    /// Business Exceptions are not logged in error but are logged in debug only.
    /// </summary>
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Conflict;

        public List<string> SupportingMessages { get; set; } = new List<string>();
        public BusinessException(string message) : base(message)
        {

        }
        public BusinessException(string message, HttpStatusCode? httpStatusCode, List<string> supportingMessages = null) : base(message)
        {
            if (httpStatusCode != null)
            {
                StatusCode = httpStatusCode.Value;
            }
            if (SupportingMessages != null)
            {
                this.SupportingMessages = supportingMessages;
            }
        }

    }


    public class CriticalBusinessException : BusinessException
    {
        public  string Logmessage { get; init; }
        public CriticalBusinessException(string message,string logmessage = null) : base(message)
        {
            Logmessage = logmessage;
        }
    }
}
