using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KWeb.Server
{
    internal static class RequestParser
    {
        public static HttpRequest Parse(Stream stream)
        {
            var method = Parse(stream, ParsingState.Method);
            var uri = Parse(stream, ParsingState.Uri);
            var request = new HttpRequest
            {
                Method = method,
                Uri = uri,
                Header = new Dictionary<string, string>(),
                RequestStream = stream
            };
            while (true)
            {
                var key = Parse(stream, ParsingState.HeaderKey);
                if (string.IsNullOrEmpty(key))
                    break;
                var value = Parse(stream, ParsingState.HeaderValue);
                request.Header.Add(key, value);
            }

            return request;
        }
        
        private enum ParsingState
        {
            Method,
            Uri,
            HeaderKey,
            HeaderValue
        }

        private static string Parse(Stream stream, ParsingState st)
        => st switch
        {
            ParsingState.Method => ParseMethod(stream),
            ParsingState.Uri => ParseUri(stream),
            ParsingState.HeaderKey => ParseHeaderKey(stream),
            ParsingState.HeaderValue => ParseHeaderValue(stream),
            _ => throw new ApplicationException(),
        };

        private static string ParseHeaderValue(Stream stream)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var ch = stream.ReadByte();
                if (ch < 0 ||
                    ch > 255)
                    throw new HttpException(400);
                if (ch == ' ' &&
                    sb.Length == 0)
                    continue;
                if (ch == '\r')
                {
                    ch = stream.ReadByte();
                    if (ch != '\n')
                        throw new HttpException(400);
                    break;
                }

                sb.Append((char)ch);
            }

            return sb.ToString();
        }

        private static string ParseHeaderKey(Stream stream)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var ch = stream.ReadByte();
                if (ch < 0 ||
                    ch > 255)
                    throw new HttpException(400);
                if (ch == ':')
                    break;
                if (ch == '\r')
                {
                    if (sb.Length != 0)
                        throw new HttpException(400);
                    ch = stream.ReadByte();
                    if (ch != '\n')
                        throw new HttpException(400);
                    break;
                }

                sb.Append((char)ch);
            }

            return sb.ToString();
        }

        private static string ParseUri(Stream stream)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var ch = stream.ReadByte();
                if (ch < 0 ||
                    ch > 255)
                    throw new HttpException(400);
                if (ch == '\r')
                {
                    ch = stream.ReadByte();
                    if (ch != '\n')
                        throw new HttpException(400);
                    break;
                }

                sb.Append((char)ch);
            }

            if (sb.ToString(sb.Length - 8, 8) != "HTTP/1.1")
                throw new HttpException(505);
            return sb.ToString(0, sb.Length - 9);
        }

        private static string ParseMethod(Stream stream)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var ch = stream.ReadByte();
                if (ch < 0 ||
                    ch > 255)
                    throw new HttpException(400);
                if (ch == ' ')
                    break;
                sb.Append((char)ch);
            }

            switch (sb.ToString())
            {
                case "OPTIONS":
                case "GET":
                case "HEAD":
                case "POST":
                case "PUT":
                case "DELETE":
                case "TRACE":
                case "CONNECT":
                    return sb.ToString();
                default:
                    throw new HttpException(400);
            }
        }
    }
}