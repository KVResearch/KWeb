using System;
using System.IO;
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
                "Mthod => " + request.Method + "\n" +
                "Uri   => " + request.Uri + "\n" +
                "Data  => " + request.ReadToEnd()
            );
        }
    }
}