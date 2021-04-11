using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KWeb.Server
{
    public static class ResponseCodes
    {
        public static readonly ConcurrentDictionary<int, string> Codes
            = new ConcurrentDictionary<int, string>(
                new List<KeyValuePair<int, string>>
                {
                    new KeyValuePair<int, string>(100, "Continue"),
                    new KeyValuePair<int, string>(101, "Switching Protocols"),
                    new KeyValuePair<int, string>(200, "OK"),
                    new KeyValuePair<int, string>(201, "Created"),
                    new KeyValuePair<int, string>(202, "Accepted"),
                    new KeyValuePair<int, string>(203, "Non-Authoritative Information"),
                    new KeyValuePair<int, string>(204, "No Content"),
                    new KeyValuePair<int, string>(205, "Reset Content"),
                    new KeyValuePair<int, string>(206, "Partial Content"),
                    new KeyValuePair<int, string>(300, "Multiple Choices"),
                    new KeyValuePair<int, string>(301, "Moved Permanently"),
                    new KeyValuePair<int, string>(302, "Found"),
                    new KeyValuePair<int, string>(303, "See Other"),
                    new KeyValuePair<int, string>(304, "Not Modified"),
                    new KeyValuePair<int, string>(305, "Use Proxy"),
                    new KeyValuePair<int, string>(307, "Temporary Redirect"),
                    new KeyValuePair<int, string>(400, "Bad Request"),
                    new KeyValuePair<int, string>(401, "Unauthorized"),
                    new KeyValuePair<int, string>(402, "Payment Required"),
                    new KeyValuePair<int, string>(403, "Forbidden"),
                    new KeyValuePair<int, string>(404, "Not Found"),
                    new KeyValuePair<int, string>(405, "Method Not Allowed"),
                    new KeyValuePair<int, string>(406, "Not Acceptable"),
                    new KeyValuePair<int, string>(407, "Proxy Authentication Required"),
                    new KeyValuePair<int, string>(408, "Request Timeout"),
                    new KeyValuePair<int, string>(409, "Conflict"),
                    new KeyValuePair<int, string>(410, "Gone"),
                    new KeyValuePair<int, string>(411, "Length Required"),
                    new KeyValuePair<int, string>(412, "Precondition Failed"),
                    new KeyValuePair<int, string>(413, "Request Entity Too Large"),
                    new KeyValuePair<int, string>(414, "Request-URI Too Long"),
                    new KeyValuePair<int, string>(415, "Unsupported Media Type"),
                    new KeyValuePair<int, string>(416, "Requested Range Not Satisfiable"),
                    new KeyValuePair<int, string>(417, "Expectation Failed"),
                    new KeyValuePair<int, string>(418, "I'm a teapot"),
                    new KeyValuePair<int, string>(500, "Internal Server Error"),
                    new KeyValuePair<int, string>(501, "Not Implemented"),
                    new KeyValuePair<int, string>(502, "Bad Gateway"),
                    new KeyValuePair<int, string>(503, "Service Unavailable"),
                    new KeyValuePair<int, string>(504, "Gateway Timeout"),
                    new KeyValuePair<int, string>(505, "HTTP Version Not Supported")
                });

        public static string Get(int code) => Codes[code];
    }
}