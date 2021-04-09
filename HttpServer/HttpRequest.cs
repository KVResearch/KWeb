using System.Collections.Generic;
using System.IO;
using System.Net;

namespace KWeb.Server
{
    public class HttpRequest
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public string BaseUri { get; set; }
        public EndPoint RemoteAddress { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public Stream RequestStream { get; set; }
    }
}