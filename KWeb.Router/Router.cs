using System.Collections.Concurrent;

using KWeb.Server;

namespace KWeb.Router
{
    public class Router
    {
        private ConcurrentDictionary<string, IRouteEndPoint> _routemap;

        public Router()
        {
            _routemap = new ConcurrentDictionary<string, IRouteEndPoint>();
        }

        public RouteResult Route(HttpRequest request)
        {
            string host = request.Host;
            if (_routemap.ContainsKey(host))
            {
                _routemap[host].Process(request);
                return RouteResult.Success;
            }

            if (!_routemap.ContainsKey("default")) return RouteResult.Failed;
            _routemap["default"].Process(request);
            return RouteResult.Fallback;
        }

        public Router AddOrUpdateRoute(string host, IRouteEndPoint endpoint)
        {
            _routemap.AddOrUpdate(host, endpoint, (_, _) => endpoint);
            return this;
        }

        public Router RemoveRoute(string host)
        {
            _routemap.TryRemove(host, out _);
            return this;
        }

        public Router Clear()
        {
            _routemap.Clear();
            return this;
        }
    }
}