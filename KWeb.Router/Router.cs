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

        public RouteResult Route(HttpRequest request, bool autoFallback = true)
        {
            string host = request.Host.ToLower();
            if (_routemap.ContainsKey(host))
            {
                return new RouteResult
                {
                    Status = RouteResultType.Success,
                    Result = _routemap[host].Process(request)
                };
            }

            if (autoFallback && _routemap.ContainsKey("default"))
                return new RouteResult
                {
                    Status = RouteResultType.Fallback,
                    Result = _routemap["default"].Process(request)
                };

            return new RouteResult
            {
                Status = RouteResultType.Failed,
                Result = null
            };
        }

        public Router AddOrUpdateRoute(string host, IRouteEndPoint endpoint)
        {
            _routemap.AddOrUpdate(host.ToLower(), endpoint, (x, y) => endpoint);
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