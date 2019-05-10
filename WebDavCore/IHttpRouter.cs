using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavCore
{
    public interface IHttpRouter
    {
        IHttpEndPoint EndPoint { get; }

        bool IsEndPoint(string uri);
    }
}
