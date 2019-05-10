using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavCore
{
    public interface IHttpEndPoint
    {
        HttpResponse OnRequest(HttpRequest request);
    }
}
