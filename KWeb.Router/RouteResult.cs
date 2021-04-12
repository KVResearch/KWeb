using KWeb.Server;
using KWeb.Server.Entities;

namespace KWeb.Router
{
    public class RouteResult
    {
        public RouteResultType Status{ get; set; }
        public HttpResponse Result{ get; set; }
    }
    public enum RouteResultType
    {
        Success,
        Failed,
        Fallback
    }
}