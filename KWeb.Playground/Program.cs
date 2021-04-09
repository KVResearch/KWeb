using System;
using System.Net;
using KWeb.Router;
using KWeb.Server;

// ReSharper disable once FunctionNeverReturns

namespace KWeb.Playground
{
    class Program
    {
        private static Router.Router router;

        static void Main(string[] args)
        {
            HttpServer hs = new HttpServer("127.0.0.1", 80);
            hs.OnHttpRequest += Hs_OnHttpRequest;
            router = new Router.Router();
            router.AddOrUpdateRoute("default", new BasicRoutedEndPoint())
                .AddOrUpdateRoute("kevintest.com", new HelloRoutedEndPoint());

            hs.Start();
            while (true)
            {
                // Keep Server Working
            }
        }

        private static HttpResponse Hs_OnHttpRequest(HttpRequest request)
        {
            var r = router.Route(request, false);
            Console.WriteLine(request.Host + " => " + r.Result);
            if (r.Result != null)
                return r.Result;
            if (request.Uri.EndsWith("404"))
                return HttpException.GetExpResponse(404);
            return HttpUtil.GenerateHttpResponse(
                "Unknown Route", 503);
        }
    }

    class BasicRoutedEndPoint : IRouteEndPoint
    {
        public HttpResponse Process(HttpRequest request)
        {
            return HttpUtil.GenerateHttpResponse(
                "<h1>Hello from KWeb.Server</h1>\n" +
                "<p>Method => " + request.Method + "</p>\n" +
                "<p>Host   => " + request.Host + "</p>\n" +
                "<p>Uri    => " + request.Uri + "</p>\n" +
                "<p>IP     => " + ((IPEndPoint) request.RemoteAddress).Address + "</p>\n" +
                "<p>Data   => " + request.ReadToEnd() + "</p>"
                , 200, "text/html");
        }
    }

    class HelloRoutedEndPoint : IRouteEndPoint
    {
        public HttpResponse Process(HttpRequest request)
        {
            return HttpUtil.GenerateHttpResponse(
                "Hello :)"
                , 200, "text/html");
        }
    }
}