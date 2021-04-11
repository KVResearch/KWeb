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
        private CertCollection _certCollect;
        private bool _enableSsl;

        public bool IsEnableSsl
        {
            get => _enableSsl;
            set
            {
                _enableSsl = value;
                if (_enableSsl && _certCollect == null)
                    _certCollect = new CertCollection();
            }
        }

        public HttpServer SetSslStatus(bool isEnable)
        {
            IsEnableSsl = isEnable;
            return this;
        }

        public bool IsRunning => m_ListenerThread is null ? false : m_ListenerThread.IsAlive;

        public delegate HttpResponse OnHttpRequestEventHandler(HttpRequest request);

        public event OnHttpRequestEventHandler OnHttpRequest;


        public HttpServer OperateCertCollection(Action<CertCollection> act)
        {
            act.Invoke(_certCollect);
            return this;
        }

        #region IP & Port

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

        #endregion

        #region Constructor

        public HttpServer(IPAddress ip, int port, bool isEnable = false)
        {
            _ip = ip;
            _port = port;
            IsEnableSsl = isEnable;
            InitialiseInstance();
        }

        public HttpServer(string ip, int port, bool isEnable = false)
        {
            _ip = IPAddress.Parse(ip);
            _port = port;
            IsEnableSsl = isEnable;
            InitialiseInstance();
        }

        #endregion

        private void InitialiseInstance()
        {
            m_Listener = new TcpListener(_ip, _port);
        }

        #region Operation

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

        #endregion

        private void MainProcess()
        {
            m_Listener.Start();
            while (true)
            {
                var tcp = m_Listener.AcceptTcpClient();
                Task.Run(() => Process(tcp));
            }
        }

        #region SSL

        private Stream ProcessSsl(Stream clientStream, int readTimeout = 10000, int writeTimeout = 10000)
        {
            string host = null;
            // No need to catch exp, will catch in method Process
            SslStream sslStream = new SslStream(clientStream, false, null,
                (obj, targetHost, certCollect, cert, acptIssuer) =>
                {
                    // Not getting here :(
                    host = targetHost;
                    return null;
                });

            sslStream.AuthenticateAsServer(GetCertByHostname(host),
                false,
                SslProtocols.Tls13 | SslProtocols.Tls12, true);
            sslStream.ReadTimeout = readTimeout;
            sslStream.WriteTimeout = writeTimeout;
            return sslStream;
        }

        private X509Certificate GetCertByHostname(string host)
        {
            var cert = _certCollect.GetPossibleCert(host);
            if (cert is null)
                throw new ArgumentNullException($"Cannot find certification for {host}");
            return cert;
        }

        #endregion

        private void Process(TcpClient tcp)
        {
            try
            {
                Stream stream = tcp.GetStream();
                if (IsEnableSsl)
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