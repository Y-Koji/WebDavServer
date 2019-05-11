using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class HttpResponse : IDisposable
    {
        public int Status { get; private set; }
        public MediaType MediaType { get; private set; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public long ContentLength { get; private set; }
        private Stream Body { get; set; }

        private HttpResponse() { }
        
        public static HttpResponse FromString(string body, int status = 200)
        {
            HttpResponse response = new HttpResponse();
            response.Status = status;
            response.MediaType = MediaType.FromFile("test.txt");
            response.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            response.ContentLength = Encoding.UTF8.GetByteCount(body);

            return response;
        }

        public static HttpResponse FromHtml(string html, int status = 200)
        {
            HttpResponse response = new HttpResponse();
            response.Status = status;
            response.MediaType = MediaType.FromFile("test.html");
            response.Body = new MemoryStream(Encoding.UTF8.GetBytes(html));
            response.ContentLength = Encoding.UTF8.GetByteCount(html);

            return response;
        }

        public static HttpResponse FromFile(string fileName, int status = 200)
        {
            HttpResponse response = new HttpResponse();
            response.Status = status;
            response.MediaType = MediaType.FromFile(fileName);
            response.Body = File.OpenRead(fileName);
            response.ContentLength = new FileInfo(fileName).Length;

            return response;
        }

        public async Task ResponseClient(TcpClient client)
        {
            HttpStatus status = HttpStatus.Parse(Status);

            Headers["Content-Length"] = ContentLength.ToString();
            Headers["Content-Type"] = MediaType.Value;

            Stream stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream) { AutoFlush = true, NewLine = "\r\n", };
            await sw.WriteLineAsync(string.Format("HTTP/1.1 {0} {1}", status.Code, status.Message));
            foreach (var header in Headers)
            {
                string headerLine = string.Format("{0}: {1}", header.Key, header.Value);
                await sw.WriteLineAsync(headerLine);
                Debug.WriteLine("> " + headerLine);
            }

            await sw.WriteLineAsync();
            await sw.FlushAsync();
            await Body.CopyToAsync(stream);
            await stream.FlushAsync();

            stream.Dispose();
            Body.Seek(0, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            Body?.Dispose();
        }
    }
}
