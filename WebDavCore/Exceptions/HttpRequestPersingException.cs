using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavCore.Exceptions
{
    public class HttpRequestPersingException : Exception
    {
        public HttpRequestPersingException(string message) : base(message) { }
    }
}
