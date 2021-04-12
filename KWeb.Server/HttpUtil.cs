using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        public static HttpResponse GenerateHttpFileResponse(string uri, string root,
            string indexFileName = "index.html", bool isShowDic = true)
        {
            // See more:
            // https://github.com/qinyuanpei/HttpServer/blob/f5decb7b887b3afe1b9ec55b29b8a73112851bbd/HTTPServer/HTTPServer/ExampleServer.cs#L43-L87
            uri = System.Web.HttpUtility.HtmlDecode(uri).Replace(@"/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(root, uri);

            if (File.Exists(requestFile))
                return GenerateHttpFileResponse(requestFile);

            if (!Directory.Exists(requestFile))
                throw new HttpException(404);

            string index = Path.Combine(requestFile, indexFileName);

            if (File.Exists(index))
                return GenerateHttpFileResponse(index);

            return isShowDic
                ? GenerateHttpResponse(GetDirectoryListHtml(requestFile, uri), 200, "text/html;charset=utf-8")
                : HttpException.GetExpResponse(503);
        }

        private static string GetLastLevelUri(string uri, bool isLastSlashRequired = false)
        {
            uri = System.Web.HttpUtility.HtmlDecode(uri.Trim());
            uri = uri.Trim('/');
            StringBuilder sb = new StringBuilder();
            var split = uri.Split('/');

            if (split.Length == 1)
                return isLastSlashRequired
                    ? ""
                    : "/";

            foreach (var s in split[..^1])
            {
                sb.Append("/").Append(s);
            }

            if (isLastSlashRequired)
                sb.Append("/");

            return sb.ToString();
        }

        private static string GetDirectoryListHtml(string dir, string uri = null, bool isShowCopyright = true,
            string title = "File Watcher")
        {
            StringBuilder sb = new StringBuilder();

            string link = "";
            if (uri != null)
                link = GetLastLevelUri(uri, true);

            // Head
            sb.Append("<h1>")
                .Append(title)
                .Append("</h1><hr>")
                .Append("<a href=\"")
                .Append(link == "" ? "/" : link)
                .Append("\">../</a><br>");

            var dic = Directory.GetDirectories(dir);

            int length = dir.Length;
            if (!dir.EndsWith("\\") && !dir.EndsWith("/"))
                ++length;

            // TODO: Before, replace is under baseUri, so why?
            uri = uri?.Replace("\\", "/");

            var baseUri = uri is null or "/" or ""
                ? ""
                : uri.EndsWith("/")
                    ? uri
                    : uri + "/";

            int fileCount = 0;
            foreach (var p in dic)
            {
                ++fileCount;
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
                ++fileCount;
                var s = p.Replace("\\", "/").Substring(length);
                sb.Append("<a href=\"/")
                    .Append(baseUri)
                    .Append(s)
                    .Append("\">")
                    .Append(s)
                    .Append("</a>")
                    .Append("\n<br>\n");
            }

            if (fileCount == 0)
                sb.Append(@"<p>Empty</p>");

            if (isShowCopyright)
                sb.Append("<hr>by ")
                    .Append(Info.ServerName)
                    .Append(' ')
                    .Append(Info.Version);

            return sb.ToString();
        }
    }
}