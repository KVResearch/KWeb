using KWeb.Server.Entities;

namespace KWeb.Router
{
    public interface IRouteEndPoint
    {
        public HttpResponse Process(HttpRequest request);
    }
}