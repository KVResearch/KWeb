using System;
using System.Collections;
using System.Collections.Concurrent;
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
            => AddOrUpdate(GetCnFromSubject(cert.Subject), cert);

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
    }
}