using System;

namespace KWeb.Server
{
    public class HttpException : Exception
    {
        public int ResponseCode { get; }

        public HttpException(int code)
        {
            ResponseCode = code;
        }

        public static HttpResponse GetExpResponse(int code, string msg = null)
        {
            return HttpUtil.GenerateHttpResponse(
                $@"<center><h1>{code} {ResponseCodes.Codes[code]}</h1><hr>{Info.ServerName} {Info.Version}</center><br>{msg}", code, "text/html");
        }
    }
}