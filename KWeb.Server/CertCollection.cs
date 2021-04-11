using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace KWeb.Server
{
    public class CertCollection : IEnumerable
    {
        private ConcurrentDictionary<string, X509Certificate> _dic;

        public CertCollection()
        {
            _dic = new ConcurrentDictionary<string, X509Certificate>();
        }

        public CertCollection AddOrUpdate(string hostname, X509Certificate cert)
        {
            if (string.IsNullOrEmpty(hostname))
                throw new ArgumentNullException("hostname is invalid!");
            _dic.AddOrUpdate(hostname, cert, (x, y) => cert);
            return this;
        }

        public CertCollection AddOrUpdate(X509Certificate cert)
        {
            X509Certificate2 x2 = new X509Certificate2(cert);
            var san = GetSubjectAlternativeNames(x2);
            if (san == null || san.Count == 0)
                throw new ArgumentNullException("Cannot find any records");
            foreach (var hostname in san)
            {
                AddOrUpdate(GetCnFromSubject(hostname.Trim()), x2);
            }

            return this;
        }

        public static List<string> GetSubjectAlternativeNames(X509Certificate2 cert)
        {
            foreach (X509Extension extension in cert.Extensions)
            {
                AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                if (string.Equals(extension.Oid.FriendlyName, "Subject Alternative Name"))
                {
                    return new List<string>(
                        // FIXME: \r\n?
                        asndata.Format(true).Split(new [] {"\r\n", "DNS Name="},
                            StringSplitOptions.RemoveEmptyEntries));
                }
            }

            return null;
        }

        public CertCollection Remove(string hostname)
        {
            _dic.TryRemove(hostname, out _);
            return this;
        }

        public X509Certificate this[string hostname]
            => _dic[hostname];

        private string GetCnFromSubject(string subject)
        {
            if (subject == null)
                throw new ArgumentNullException("Subject cannot be null");

            var l = subject.Split(',');
            foreach (var i in l)
            {
                var s = i.Trim();
                var ss = s.Split('=');
                if (ss.Length != 2)
                    continue;
                if (ss[0].Trim() == "CN")
                    return ss[1].Trim();
            }

            return null;
        }

        public IEnumerator GetEnumerator()
            => _dic.GetEnumerator();

        public static string[] ParseToValidCN(string hostname)
        {
            List<string> s = new List<string>();
            if (hostname.StartsWith("."))
                hostname = hostname.Substring(1);
            s.Add(hostname);
            s.Add("*." + hostname);

            var index = hostname.IndexOf('.');
            if (index > 0)
                s.Add("*" + hostname.Substring(index));
            return s.ToArray();
        }

        public X509Certificate GetPossibleCert(string host)
        {
            host = host.Trim();
            if (string.IsNullOrEmpty(host))
                return null;

            var s = ParseToValidCN(host);
            if (s.Length <= 0)
                return null;
            foreach (var i in s)
            {
                if (_dic[i] != null)
                    return _dic[i];
            }

            return null;
        }
    }
}