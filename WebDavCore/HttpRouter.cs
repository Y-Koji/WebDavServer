using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavCore
{
    public class HttpRouter : IHttpRouter
    {
        public IHttpEndPoint EndPoint { get; private set; } = null;
        private Func<string, bool> IsEndPointFunc { get; set; } = _ => false;

        public static IHttpRouter CreateRouter(string path, IHttpEndPoint endpoint)
        {
            HttpRouter router = new HttpRouter();
            router.IsEndPointFunc = param => param == path;
            router.EndPoint = endpoint;

            return router;
        }

        public bool IsEndPoint(string path)
        {
            return IsEndPointFunc(path);
        }
    }
}
