using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebDavCore
{
    public class MediaType
    {
        public static IDictionary<string, string> MimeTypes { get; } = Json.Deserialize<IDictionary<string, string>>(File.ReadAllBytes("Resources\\mime.json"));
        
        public string Extension { get; private set; }
        public string Value { get; private set; }

        public static MediaType FromFile(string fileName)
        {
            MediaType mediaType = new MediaType();
            mediaType.Extension = Path.GetExtension(fileName);
            mediaType.Value = MimeTypes[mediaType.Extension];

            return mediaType;
        }
    }
}
