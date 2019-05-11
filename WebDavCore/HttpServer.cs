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
        public IList<IHttpEndPoint> EndPoints { get; } = new List<IHttpEndPoint>();

        public HttpServer(IPAddress address, ushort port)
        {
            this.Server = new SocketServer(address, port);
        }

        public async Task Start(CancellationToken token)
        {
            await Server.Start(async client =>
            {
                HttpResponse response = null;

                try
                {
                    HttpRequest request = await HttpRequest.ParseAsync(client);
                    
                    foreach (var endpoint in EndPoints)
                    {
                        if (endpoint.OnPathMatch(request.Path))
                        {
                            response = endpoint.OnRequest(request);

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
