using KWeb.Server;

namespace KWeb.Router
{
    public interface IRouteEndPoint
    {
        public HttpResponse Process(HttpRequest request);
    }
}