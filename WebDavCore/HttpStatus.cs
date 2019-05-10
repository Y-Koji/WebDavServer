using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebDavCore
{
    public class HttpStatus
    {
        public static IDictionary<int, string> CodeList = Json.Deserialize<IDictionary<int, string>>(File.ReadAllBytes("Resources\\status.json"));
        public int Code { get; private set; }
        public string Message { get; private set; }

        public static HttpStatus Parse(int code)
        {
            HttpStatus status = new HttpStatus();
            status.Code = code;
            status.Message = CodeList[code];
            
            return status;
        }
    }
}
