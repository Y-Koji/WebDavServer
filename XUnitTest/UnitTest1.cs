using System;
using System.Net;
using System.Text;
using WebDavCore;
using WebDavCore.EndPoint;
using Xunit;

namespace XUnitTest
{
    public class Root : IHttpEndPoint
    {
        public bool OnPathMatch(string path) => path == "/";

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
            http.EndPoints.Add(new StaticResourceEndPoint(@"E:\", ".*"));
            //http.Routers.Add(new StaticResourceRouter(".*", @"E:\", "(.*)"));
            //http.Routers.Add(new StaticResourceRouter("^/video/.*", @"E:\Video\_Record", "^/video/(.*)"));
            http.Start().Wait();

            http.Dispose();
        }
    }
}
