using System;
using System.Net;
using System.Text;
using WebDavCore;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            HttpServer http = new HttpServer(IPAddress.Any, 8080);
            http.Start(request =>
            {
                return new HttpResponse
                {
                    Status = 200,
                    ContentType = "text/plain",
                    Body = Encoding.UTF8.GetBytes("Hello, World!"),
                };
            }).Wait();

            http.Dispose();
        }
    }
}
