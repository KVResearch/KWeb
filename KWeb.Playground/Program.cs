using System;
using System.IO;
using System.Net;
using KWeb.Server;

namespace KWeb.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer hs = new HttpServer("127.0.0.1", 80);
            hs.OnHttpRequest += Hs_OnHttpRequest;
            hs.Start();
            while (true)
            {
                // Keep Server Working
            }
        }

        private static HttpResponse Hs_OnHttpRequest(HttpRequest request)
        {
            return HttpUtil.GenerateHttpResponse(
                "<h1>Hello from KWeb.Server</h1>\n" +
                "<p>Method => " + request.Method + "</p>\n" +
                "<p>Uri    => " + request.Uri + "</p>\n" +
                "<p>IP     => " + ((IPEndPoint)request.RemoteAddress).Address + "</p>\n" +
                "<p>Data   => " + request.ReadToEnd() + "</p>"
            , 200, "html");
        }
    }
}