using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class SocketServer : IDisposable
    {
        private TcpListener Listener { get; }

        public SocketServer(IPAddress address, ushort port)
        {
            this.Listener = new TcpListener(address, port);
        }
        
        public async Task Start(Action<TcpClient> action, CancellationToken token)
        {
            Listener.Start();

            token.Register(() => Stop());

            while (!token.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await Listener.AcceptTcpClientAsync();

                    Task.Run(() => action?.Invoke(client)).Dispose();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception: SocketServer#Start");
                    Debug.WriteLine(e);
                }
            }
        }

        public async Task Start(Action<TcpClient> action) => await Start(action, CancellationToken.None);

        public void Stop()
        {
            Listener.Stop();
        }

        public void Dispose()
        {
            Listener.Stop();
        }
    }
}
