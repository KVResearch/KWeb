using System;

namespace KWeb.Server.Entities
{
    public class HttpException : Exception
    {
        public int ResponseCode { get; }

        public override string Message { get; }

        public HttpException(int code, string message = null)
        {
            ResponseCode = code;
            Message = message;
        }

        public string ToHtml()
        {
            return
                $@"<center><h1>{ResponseCode} {ResponseCodes.Codes[ResponseCode]}</h1><hr>{Info.ServerName} {Info.Version}</center><br>{Message}";
        }

        public HttpResponse ToHttpResponse()
            => GetExpResponse(ResponseCode, Message);


        public static HttpResponse GetExpResponse(int code, string msg = null)
        {
            return KWeb.Server.HttpUtil.GenerateHttpResponse(
                $@"<center><h1>{code} {ResponseCodes.Codes[code]}</h1><hr>{Info.ServerName} {Info.Version}</center><br>{msg}",
                code, "text/html");
        }
    }
}