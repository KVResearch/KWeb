using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable FunctionNeverReturns

namespace KWeb.Server
{
    // ReSharper disable once UnusedMember.Global
    public class HttpServer
    {
        private IPAddress _ip;
        private int _port;
        private Thread m_ListenerThread;
        private TcpListener m_Listener;

        public HttpServer SetIp(IPAddress ip, bool reinitialise = true)
        {
            if (IsRunning)
                throw new InvalidOperationException("Operation cannot be done when server is running!");
            _ip = ip;
            if (reinitialise)
                InitialiseInstance();
            return this;
        }

        public HttpServer SetIp(string ip, bool reinitialise = true)
            => SetIp(IPAddress.Parse(ip), reinitialise);

        public HttpServer SetPort(int port, bool reinitialise = true)
        {
            if (IsRunning)
                throw new InvalidOperationException("Operation cannot be done when server is running!");
            _port = port;
            if (reinitialise)
                InitialiseInstance();
            return this;
        }

        public delegate HttpResponse OnHttpRequestEventHandler(HttpRequest request);

        public event OnHttpRequestEventHandler OnHttpRequest;

        public HttpServer(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
            InitialiseInstance();
        }

        public HttpServer(string ip, int port)
        {
            _ip = IPAddress.Parse(ip);
            _port = port;
            InitialiseInstance();
        }

        private void InitialiseInstance()
        {
            m_Listener = new TcpListener(_ip, _port);
        }

        public bool IsRunning => m_ListenerThread is null ? false : m_ListenerThread.IsAlive;

        public void Start()
        {
            Abort();

            m_ListenerThread = new Thread(MainProcess)
            {
                IsBackground = true,
                Name = Info.ServerName
            };

            m_ListenerThread.Start();
        }

        public void Abort()
        {
            try
            {
                m_ListenerThread?.Abort();
            }
            catch
            {
                // Ignore
            }

            m_Listener.Stop();
        }

        private void MainProcess()
        {
            m_Listener.Start();
            while (true)
            {
                var tcp = m_Listener.AcceptTcpClient();
                Task.Run(() => Process(tcp));
            }
        }

        public bool IsSsl => _cert is null;

        private X509Certificate _cert;

        public HttpServer SetSsl(string certificate)
            => SetSsl(X509Certificate.CreateFromCertFile(certificate));

        public HttpServer SetSsl(X509Certificate certifiate)
        {
            _cert = certifiate;
            return this;
        }


        private Stream ProcessSsl(Stream clientStream)
        {
            try
            {
                SslStream sslStream = new SslStream(clientStream);
                sslStream.AuthenticateAsServer(_cert,
                    false,
                    SslProtocols.Tls13 | SslProtocols.Tls12 , true);
                sslStream.ReadTimeout = 10000;
                sslStream.WriteTimeout = 10000;
                return sslStream;
            }
            catch (Exception e)
            {
                clientStream.Close();
                throw new HttpException(500);
            }

            return null;
        }

        private void Process(TcpClient tcp)
        {
            try
            {
                Stream stream = tcp.GetStream();
                if (IsSsl)
                    stream = ProcessSsl(stream);
                HttpResponse response;
                try
                {
                    var request = RequestParser.Parse(stream);
                    if (OnHttpRequest == null)
                        throw new HttpException(501);
                    // TODO: process to E.P. is nonsense
                    request.RemoteAddress = tcp.Client.RemoteEndPoint;
                    response = OnHttpRequest(request);
                    request.DisposeHeader();
                }
                catch (HttpException e)
                {
                    response = HttpException.GetExpResponse(e.ResponseCode);
                }
                catch (Exception e)
                {
                    response = HttpUtil.GenerateHttpResponse(e.ToString(), 500, "text/plain");
                }

                try
                {
                    using (response)
                        ResponseWriter.Write(stream, response);
                }
                catch
                {
                    // ignored
                }

                stream.Close();
                stream.Dispose();
            }
            catch
            {
                // ignored
            }
            finally
            {
                tcp.Close();
                tcp.Dispose();
            }
        }
    }
}