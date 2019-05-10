using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class HttpResponse
    {
        public int Status { get; set; } = 200;
        public string ContentType { get; set; } = string.Empty;
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public byte[] Body { get; set; } = new byte[0];

        public async Task ResponseClient(TcpClient client)
        {
            HttpStatus status = HttpStatus.Parse(Status);

            Headers["Content-Length"] = Body.Length.ToString();
            Headers["Content-Type"] = ContentType;

            Stream stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream);
            await sw.WriteAsync(string.Format("HTTP/1.1 {0} {1}\r\n", status.Code, status.Message));
            foreach (var header in Headers)
            {
                await sw.WriteAsync(string.Format("{0}: {1}\r\n", header.Key, header.Value));
            }

            await sw.WriteAsync("\r\n");
            await sw.FlushAsync();
            await stream.WriteAsync(Body, 0, Body.Length);
            await stream.FlushAsync();
        }
    }
}
