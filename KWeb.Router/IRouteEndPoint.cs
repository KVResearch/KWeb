using KWeb.Server;

namespace KWeb.Router
{
    public interface IRouteEndPoint
    {
        public void Process(HttpRequest request);
    }
}