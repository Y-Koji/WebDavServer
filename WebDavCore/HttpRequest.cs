using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebDavCore
{
    public class HttpRequest
    {
        public string Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }

        public TcpClient Client { get; private set; }

        public static async Task<HttpRequest> ParseAsync(TcpClient client)
        {
            HttpRequest request = new HttpRequest();
            request.Client = client;

            StreamReader sr = new StreamReader(client.GetStream());
            string firstLine = await sr.ReadLineAsync();
            request.Method = firstLine.Split(' ')[0];
            request.Path = firstLine.Split(' ')[1];
            request.Version = firstLine.Split(' ')[2];
            Debug.WriteLine("< " + firstLine);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            for (string line = sr.ReadLine();!string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
            {
                string key = line.Split(':')[0].Trim();
                string value = line.Split(':')[1].Trim();

                headers.Add(key, value);

                Debug.WriteLine("< " + line);
            }

            request.Headers = headers;
            Debug.WriteLine("");
            
            return request;
        }
    }
}
