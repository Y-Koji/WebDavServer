using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class HttpServer : IDisposable
    {
        private SocketServer Server { get; }
        public IList<IHttpRouter> Routers { get; } = new List<IHttpRouter>();

        public HttpServer(IPAddress address, ushort port)
        {
            this.Server = new SocketServer(address, port);
        }

        public async Task Start(CancellationToken token)
        {
            await Server.Start(async client =>
            {
                HttpRequest request = await HttpRequest.ParseAsync(client);
                HttpResponse response = null;

                try
                {
                    foreach (var router in Routers)
                    {
                        if (router.IsEndPoint(request.Path))
                        {
                            response = router.EndPoint.OnRequest(request);

                            await response.ResponseClient(client);

                            break;
                        }
                    }

                    await HttpResponse.FromString("404 Not Found", 404).ResponseClient(client);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                finally
                {
                    client.Dispose();
                    response?.Dispose();
                }

            }, token);
        }

        public async Task Start() => await Start(CancellationToken.None);

        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}
