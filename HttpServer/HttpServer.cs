﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KWeb.Server
{
    // ReSharper disable once UnusedMember.Global
    public class HttpServer
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
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

        // ReSharper disable once EventNeverSubscribedTo.Global
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
            m_ListenerThread = new Thread(MainProcess)
            {
                IsBackground = true,
                Name = "HttpServer"
            };
        }

        public bool IsRunning => m_ListenerThread.IsAlive;

        // ReSharper disable once UnusedMember.Global
        public void Start()
        {
            Abort();

            m_ListenerThread.Start();
        }

        public void Abort()
        {
            try
            {
                m_ListenerThread.Abort();
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
                var thr = new Thread(o => Process((TcpClient) o));
                thr.Start(tcp);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void Process(TcpClient tcp)
        {
            try
            {
                using (var stream = tcp.GetStream())
                {
                    HttpResponse response;
                    try
                    {
                        var request = RequestParser.Parse(stream);
                        if (OnHttpRequest == null)
                            throw new HttpException(501);
                        response = OnHttpRequest(request);
                    }
                    catch (HttpException e)
                    {
                        response = new HttpResponse {ResponseCode = e.ResponseCode};
                    }
                    catch (Exception e)
                    {
                        response = HttpUtil.GenerateHttpResponse(e.ToString(), 200, "text/plain");
                        response.ResponseCode = 500;
                    }

                    using (response)
                        ResponseWriter.Write(stream, response);

                    stream.Close();
                }

                tcp.Close();
            }
            catch
            {
                // ignored
            }
        }
    }
}