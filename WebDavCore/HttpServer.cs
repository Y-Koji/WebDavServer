using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class HttpServer : IDisposable
    {
        private SocketServer Server { get; }

        public HttpServer(IPAddress address, ushort port)
        {
            this.Server = new SocketServer(address, port);
        }

        public async Task Start(Func<HttpRequest, HttpResponse> process, CancellationToken token)
        {
            await Server.Start(async client =>
            {
                HttpRequest request = await HttpRequest.ParseAsync(client);

                HttpResponse response = process?.Invoke(request);

                await response.ResponseClient(client);
            }, token);
        }

        public async Task Start(Func<HttpRequest, HttpResponse> process) => await Start(process, CancellationToken.None);

        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}
