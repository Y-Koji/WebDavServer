using System;
using System.Collections.Generic;
using System.Text;
using WebDavCore.Properties;

namespace WebDavCore
{
    public class HttpStatus
    {
        public static IDictionary<int, string> CodeList = Json.Deserialize<IDictionary<int, string>>(Resources.status);
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
