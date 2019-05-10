using System;
using System.Net;
using System.Text;
using WebDavCore;
using Xunit;

namespace XUnitTest
{
    public class Root : IHttpEndPoint
    {
        public HttpResponse OnRequest(HttpRequest request)
        {
            HttpResponse response = HttpResponse.FromFile(@"E:\Video\_Record\ScreenRecording_12-08-2018 19-43-38.mp4");

            return response;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            HttpServer http = new HttpServer(IPAddress.Any, 8080);
            http.Routers.Add(HttpRouter.CreateRouter("/", new Root()));
            http.Start().Wait();

            http.Dispose();
        }
    }
}
