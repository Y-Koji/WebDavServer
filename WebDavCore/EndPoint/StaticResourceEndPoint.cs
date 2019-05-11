using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebDavCore.EndPoint
{
    public class StaticResourceEndPoint : IHttpEndPoint
    {
        public string HomeDirectory { get; }
        public string PathPattern { get; }
        
        public StaticResourceEndPoint(string homeDir, string pathPattern = "")
        {
            this.HomeDirectory = homeDir;
            this.PathPattern = pathPattern;
        }

        public bool OnPathMatch(string path) => Regex.IsMatch(path, PathPattern);

        public HttpResponse OnRequest(HttpRequest request)
        {
            var match = Regex.Match(request.Path, PathPattern);
            var path = 1 < match.Groups.Count ? match.Groups[1].Value : match.Groups[0].Value;

            string filePath = Path.Combine(HomeDirectory, path.TrimStart('/'));
            if (request.Path.Last() == '/')
            {
                return DirectoryResponse(request, filePath);
            }
            else if (File.Exists(filePath))
            {
                return HttpResponse.FromFile(filePath);
            }
            else
            {
                return HttpResponse.FromString("404 Not Found.", 404);
            }
        }

        private HttpResponse DirectoryResponse(HttpRequest request, string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!doctype html>")
                    .AppendLine("<head>")
                    .AppendLine("<meta charset=\"utf-8\" />")
                    .AppendLine("</head>")
                    .AppendLine("<body>");

                foreach (var dir in Directory.GetDirectories(dirPath))
                {
                    sb.AppendFormat(
                        "<div><a href=\"{0}/\">{1}/</a></div>",
                        request.Path + dir.Replace(dirPath, string.Empty).Replace("\\", "/").TrimStart('/'),
                        dir.Split('\\').Last());
                }

                foreach (var file in Directory.GetFiles(dirPath))
                {
                    sb.AppendFormat(
                        "<div><a href=\"{0}\">{1}</a></div>", 
                        request.Path + file.Replace(dirPath, string.Empty).Replace("\\", "/").TrimStart('/'),
                        Path.GetFileName(file));
                }

                sb.AppendLine("</body>");

                return HttpResponse.FromHtml(sb.ToString());
            }
            else
            {
                return HttpResponse.FromString("404 Not Found.", 404);
            }
        }
    }
}
