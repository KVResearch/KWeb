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
            uri = uri.Replace(@"/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(basePath, uri);

            string extension = Path.GetExtension(requestFile);

            HttpResponse response;

            if (extension != "")
            {
                response = GenerateHttpFileResponse(requestFile);
            }
            else
            {
                string index = Path.Combine(requestFile, "index.html");
                if (Directory.Exists(requestFile) && !File.Exists(index))
                {
                    response = isShowDic
                        ? GenerateHttpResponse(ListDirectory(requestFile, uri), 200, "text/html")
                        : HttpException.GetExpResponse(503);
                }
                else
                {
                    response = GenerateHttpFileResponse(index);
                }
            }

            return response;
        }

        private static string ListDirectory(string dir, string uri = null)
        {
            StringBuilder sb = new StringBuilder();

            string link = "";
            if (uri != null)
            {
                StringBuilder ssb = new StringBuilder();
                var split = uri.Split('/');
                foreach (var s in split[..^1])
                {
                    ssb.Append("/").Append(s);
                }

                link = ssb.ToString();
            }

            // Head
            sb.Append("<a href=\"")
                .Append(link)
                .Append("\"><h1>")
                .Append(uri is null ? "../" : uri)
                .Append("/</h1></a>")
                .Append("<hr>");
            
            var dic = Directory.GetDirectories(dir);
            foreach (var p in dic)
            {
                var s = p.Replace("\\", "/").Substring(dir.Length);
                sb.Append("<a href=\"")
                    .Append(s)
                    .Append("\">")
                    .Append(s)
                    .Append("</a>")
                    .Append("\n<br>\n");
            }

            var files = Directory.GetFiles(dir);
            foreach (var p in files)
            {
                var s = p.Replace("\\", "/").Substring(dir.Length);
                sb.Append("<a hre=\"")
                    .Append(s)
                    .Append("\">")
                    .Append(s)
                    .Append("</a>")
                    .Append("\n<br>\n");
            }

            return sb.ToString();
        }
    }
}