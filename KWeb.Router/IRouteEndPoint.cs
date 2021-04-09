using KWeb.Server;

namespace KWeb.Router
{
    public interface IRouteEndPoint
    {
        public string Process(HttpRequest request);
    }
}