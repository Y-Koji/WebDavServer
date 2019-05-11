using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavCore
{
    public interface IHttpEndPoint
    {
        bool OnPathMatch(string path);

        HttpResponse OnRequest(HttpRequest request);
    }
}
