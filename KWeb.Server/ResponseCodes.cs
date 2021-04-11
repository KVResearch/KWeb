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
                    new(100, "Continue"),
                    new(101, "Switching Protocols"),
                    new(200, "OK"),
                    new(201, "Created"),
                    new(202, "Accepted"),
                    new(203, "Non-Authoritative Information"),
                    new(204, "No Content"),
                    new(205, "Reset Content"),
                    new(206, "Partial Content"),
                    new(300, "Multiple Choices"),
                    new(301, "Moved Permanently"),
                    new(302, "Found"),
                    new(303, "See Other"),
                    new(304, "Not Modified"),
                    new(305, "Use Proxy"),
                    new(307, "Temporary Redirect"),
                    new(400, "Bad Request"),
                    new(401, "Unauthorized"),
                    new(402, "Payment Required"),
                    new(403, "Forbidden"),
                    new(404, "Not Found"),
                    new(405, "Method Not Allowed"),
                    new(406, "Not Acceptable"),
                    new(407, "Proxy Authentication Required"),
                    new(408, "Request Timeout"),
                    new(409, "Conflict"),
                    new(410, "Gone"),
                    new(411, "Length Required"),
                    new(412, "Precondition Failed"),
                    new(413, "Request Entity Too Large"),
                    new(414, "Request-URI Too Long"),
                    new(415, "Unsupported Media Type"),
                    new(416, "Requested Range Not Satisfiable"),
                    new(417, "Expectation Failed"),
                    new(418, "I'm a teapot"),
                    new(500, "Internal Server Error"),
                    new(501, "Not Implemented"),
                    new(502, "Bad Gateway"),
                    new(503, "Service Unavailable"),
                    new(504, "Gateway Timeout"),
                    new(505, "HTTP Version Not Supported")
                });

        public static string Get(int code) => Codes[code];
    }
}