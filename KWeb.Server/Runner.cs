using System;
using System.Collections;
using System.Collections.Generic;

namespace KWeb.Server
{
    public class Runner : IEnumerable
    {
        private readonly string _ip;
        private Dictionary<int, HttpServer> _dic;

        public Runner(string ip)
        {
            _ip = ip;
            _dic = new Dictionary<int, HttpServer>();
        }

        public Runner Stop()
        {
            foreach (var kvp in _dic)
            {
                kvp.Value?.Abort();
            }

            return this;
        }

        public Runner Stop(int port)
        {
            _dic[port]?.Abort();
            return this;
        }

        public Runner Run()
        {
            foreach (var kvp in _dic)
            {
                kvp.Value?.Start();
            }

            return this;
        }

        public Runner Run(int port)
        {
            _dic[port]?.Start();
            return this;
        }

        public Runner Clear()
        {
            Stop();
            _dic.Clear();
            return this;
        }

        public Runner Add(int port, HttpServer.OnHttpRequestEventHandler evt, bool isEnable = false)
        {
            var hs = new HttpServer(_ip, port, isEnable);
            hs.OnHttpRequest += evt;
            _dic.Add(port, hs);
            return this;
        }

        public Runner Add(int port, bool isEnable = false)
        {
            _dic.Add(port, new HttpServer(_ip, port, isEnable));
            return this;
        }

        public Runner Add(int port, Func<HttpRequest, HttpResponse> evt, bool isEnable = false)
        {
            var hs = new HttpServer(_ip, port, isEnable);
            hs.OnHttpRequest += evt.Invoke;
            _dic.Add(port, hs);
            return this;
        }

        public Runner Remove(int port)
        {
            Stop(port);
            _dic.Remove(port);
            return this;
        }

        public IEnumerator GetEnumerator()
            => _dic.GetEnumerator();

        public HttpServer this[int port]
            => _dic[port];
    }
}