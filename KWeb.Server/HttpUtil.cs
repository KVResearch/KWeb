using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace KWeb.Server
{
    public static class HttpUtil
    {
        public static string ReadToEnd(this HttpRequest request, int maxLength = 1048576)
        {
            if (!request.Header.ContainsKey("Content-Length"))
                return null;

            var len = Convert.ToInt32(request.Header["Content-Length"]);
            if (len > maxLength)
                throw new HttpException(413);

            var buff = new byte[len];
            if (len > 0)
                request.RequestStream.Read(buff, 0, len);

            return Encoding.UTF8.GetString(buff);
        }

        public static HttpResponse GenerateHttpResponse(string str, int httpCode = 200,
            string contentType = "text/json")
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            sw.Write(str);
            sw.Flush();
            return GenerateHttpResponse(stream, httpCode, contentType);
        }

        public static HttpResponse GenerateHttpResponse(Stream stream, int httpCode = 200,
            string contentType = "text/json")
        {
            stream.Position = 0;
            return new HttpResponse
            {
                ResponseCode = httpCode,
                Header = new Dictionary<string, string>
                {
                    {"Content-Type", contentType},
                    {"Content-Length", stream.Length.ToString(CultureInfo.InvariantCulture)}
                },
                ResponseStream = stream
            };
        }

        public static HttpResponse GenerateHttpDirectResponse(string url, int httpCode = 301,
            string contentType = "text/json")
        {
            return new HttpResponse
            {
                ResponseCode = httpCode,
                Header = new Dictionary<string, string>
                {
                    {"Content-Type", contentType},
                    {"Location", url}
                }
            };
        }

        public static Stream FileToStream(string path)
        {
            Stream stream = new MemoryStream(File.ReadAllBytes(path));
            return stream;
        }

        public static HttpResponse GenerateHttpFileResponse(string filename)
        {
            if (!File.Exists(filename))
            {
                return HttpException.GetExpResponse(404);
            }

            var contentType = MimeHelper.GetContentType(Path.GetExtension(filename));
            return new HttpResponse
            {
                ResponseCode = 200,
                Header = new Dictionary<string, string>
                {
                    {"Content-Type", contentType}
                },
                ResponseStream = FileToStream(filename)
            };
        }

        public static HttpResponse GenerateHttpFileResponse(string uri, string basePath, bool isShowDic = true)
        {
            // See more:
            // https://github.com/qinyuanpei/HttpServer/blob/f5decb7b887b3afe1b9ec55b29b8a73112851bbd/HTTPServer/HTTPServer/ExampleServer.cs#L43-L87
            uri = System.Web.HttpUtility.HtmlDecode(uri).Replace(@"/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(basePath, uri);

            if (File.Exists(requestFile))
            {
                return GenerateHttpFileResponse(requestFile);
            }

            if (Directory.Exists(requestFile))
            {
                string index = Path.Combine(requestFile, "index.html");
                if (File.Exists(index))
                {
                    return GenerateHttpFileResponse(index);
                }
                else
                {
                    return isShowDic
                        ? GenerateHttpResponse(ListDirectory(requestFile, uri), 200, "text/html;charset=utf-8")
                        : HttpException.GetExpResponse(503);
                }
            }

            throw new HttpException(404);

        }

        private static string ListDirectory(string dir, string uri = null, bool isShowCopyright = true)
        {
            StringBuilder sb = new StringBuilder();

            string link = "";
            if (uri != null)
            {
                uri = System.Web.HttpUtility.HtmlDecode(uri);
                StringBuilder ssb = new StringBuilder();
                var split = uri.Split('/');

                if (split.Length == 1)
                    link = "/";
                else
                {
                    foreach (var s in split[..^1])
                    {
                        ssb.Append("/").Append(s);
                    }

                    link = ssb.ToString();
                }
            }

            // Head
            sb.Append("<a href=\"")
                .Append(link)
                .Append("\"><h1>../</h1></a><hr>");

            var dic = Directory.GetDirectories(dir);

            int length = dir.Length;
            if (!dir.EndsWith("\\") && !dir.EndsWith("/"))
                ++length;

            var baseUri = (uri is null
                ? "/"
                : uri.EndsWith("/")
                    ? uri
                    : uri + "/").Replace("\\", "/");
            if (baseUri == "/")
                baseUri = "";
            
            foreach (var p in dic)
            {
                var s = p.Replace("\\", "/").Substring(length);
                sb.Append("<a href=\"/")
                    .Append(baseUri)
                    .Append(s)
                    .Append("\">")
                    .Append(s)
                    .Append("</a>")
                    .Append("\n<br>\n");
            }

            var files = Directory.GetFiles(dir);
            foreach (var p in files)
            {
                var s = p.Replace("\\", "/").Substring(length);
                sb.Append("<a href=\"/")
                    .Append(baseUri)
                    .Append(s)
                    .Append("\">")
                    .Append(s)
                    .Append("</a>")
                    .Append("\n<br>\n");
            }

            if (isShowCopyright)
                sb.Append("<hr>by ")
                    .Append(Info.ServerName)
                    .Append(" ")
                    .Append(Info.Version);

            return sb.ToString();
        }
    }
}