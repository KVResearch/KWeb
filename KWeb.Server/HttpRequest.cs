using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace KWeb.Server
{
    public class HttpRequest : IDisposable
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public EndPoint RemoteAddress { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public string Host => Header?["Host"];
        public Stream RequestStream { get; set; }

        public void Dispose()
        {
            Header = null;
            RequestStream?.Close();
            RequestStream?.Dispose();
        }

        public void DisposeHeader()
        {
            Header = null;
        }
    }
}